using AutoMapper;
using FribergFastigheter.Server.Data.Entities;
using FribergFastigheter.Server.Data.Interfaces;
using FribergFastigheter.Server.Services;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using FribergFastigheter.Shared.Constants;
using FribergFastigheter.Shared.Dto.Housing;
using FribergFastigheter.Shared.Dto.Image;
using FribergFastigheter.Shared.Dto.Error;
using FribergFastigheter.Shared.Enums;
using FribergFastigheter.Shared.Services.AuthorizationHandlers.Data.Housing;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FribergFastigheter.Server.Controllers.BrokerFirmApi
{
    /// <summary>
    /// An API controller for the broker firm housings API.
    /// </summary>
    /// <!-- Author: Jimmie -->
    /// <!-- Co Authors: Marcus-->
    [Route("broker-firm-api")]
    [ApiController]
    public class BrokerHousingController : ControllerBase
    {
        #region Fields

        /// <summary>
        /// The injected authorization service. 
        /// </summary>
        private readonly IAuthorizationService _authorizationService;

        /// <summary>
        /// The injected Auto Mapper.
        /// </summary>
        private readonly IMapper _autoMapper;

        /// <summary>
        /// The injected broker firm repository.
        /// </summary>
        private readonly IBrokerFirmRepository _brokerFirmRepository;

        /// <summary>
        /// The injected housing repository.
        /// </summary>
        private readonly IHousingRepository _housingRepository;

        /// <summary>
        /// The injected imageService properties.
        /// </summary>
        private readonly IImageService _imageService;
        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="housingRepository">The injected housing repository.</param>
        /// <param name="mapper">The injected Auto Mapper.</param>
        /// <param name="imageService">The injected imageService properties.</param>
        /// <param name="brokerFirmRepository">The injected broker firm repository.</param>
        /// <param name="authorizationService">The injected authorization service. </param>
        public BrokerHousingController(IHousingRepository housingRepository, IMapper mapper, IImageService imageService, IBrokerFirmRepository brokerFirmRepository, IAuthorizationService authorizationService)
        {
            _housingRepository = housingRepository;
            _autoMapper = mapper;
            _imageService = imageService;
            _brokerFirmRepository = brokerFirmRepository;
            _authorizationService = authorizationService;
        }

        #endregion

        #region ApiEndPoints

        /// <summary>
        /// An API endpoint for creating housing objects. 
        /// </summary>
        /// <param name="newHousingDto">The serialized input data.</param>
        /// <!-- Author: Jimmie, Marcus -->
        /// <!-- Co Authors: -->
        [Authorize(policy: ApplicationPolicies.Broker)]
        [HttpPost("housings")]
        [ProducesResponseType<HousingDto>(StatusCodes.Status201Created)]
        [ProducesResponseType<ErrorMessageDto>(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> CreateHousing([FromBody] CreateHousingDto newHousingDto)
        {
            var brokerFirmId = int.Parse(User.FindFirst(ApplicationUserClaims.BrokerFirmId)!.Value);
            var authData = new CreateHousingAuthorizationData(newHousingBrokerId: newHousingDto.BrokerId);
            var result = await _authorizationService.AuthorizeAsync(User, authData, ApplicationPolicies.CanCreateHousingResource);

            if (result.Succeeded)
            {
                var newHousingEntity = _autoMapper.Map<Housing>(newHousingDto);
                newHousingEntity.BrokerFirm = new BrokerFirm() { BrokerFirmId = brokerFirmId };
                await _housingRepository.AddAsync(newHousingEntity);
                var finalHousingDto = _autoMapper.Map<HousingDto>(await _housingRepository.GetHousingByIdAsync(newHousingEntity.HousingId));
                _imageService.PrepareDto(HttpContext, BrokerFileController.ImageDownloadApiEndpoint, finalHousingDto);

                return CreatedAtAction(nameof(GetHousingById), new { id = newHousingEntity.HousingId }, finalHousingDto);
            }
            else
            {
                var reason = result.Failure.FailureReasons.First(x => Enum.TryParse<HousingAuthorizationFailureReasons>(x.Message, false, out _));
                return BadRequest(new ErrorMessageDto(HttpStatusCode.BadRequest, reason.Message));
            }
        }

        /// <summary>
        /// An API endpoint for creating images. 
        /// </summary>
        /// <param name="housingId">The ID of the housing object the image belongs to</param>
        /// <param name="files">A collection of uploaded image files.</param>
        /// <!-- Author: Jimmie, Marcus -->
        /// <!-- Co Authors: -->
        [Authorize(policy: ApplicationPolicies.Broker)]
        [HttpPost("housing/{housingId:int}/images")]
        [ProducesResponseType<List<ImageDto>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ErrorMessageDto>(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> CreateHousingImages([Required] int housingId, [FromForm] IFormFileCollection files)
        {
            if (files.Count == 0)
            {
                return BadRequest(new ErrorMessageDto(HttpStatusCode.BadRequest, "No files were submitted."));
            }

            var housing = await _housingRepository.GetHousingByIdAsync(housingId);

            if (housing == null)
            {
                return NotFound(new ErrorMessageDto(HttpStatusCode.BadRequest, "The referenced housing object was not found."));
            }

            var brokerFirmId = int.Parse(User.FindFirst(ApplicationUserClaims.BrokerFirmId)!.Value);
            var brokerId = int.Parse(User.FindFirst(ApplicationUserClaims.BrokerId)!.Value);
            var authData = new CreateHousingImageAuthorizationData(existingHousingBrokerFirmId: housing.BrokerFirm.BrokerFirmId,
                existingHousingBrokerId: housing.Broker.BrokerId);
            var result = await _authorizationService.AuthorizeAsync(User, authData, ApplicationPolicies.CanCreateHousingImageResource);

            if (result.Succeeded)
            {
            List<Image> imageEntities = new();

            foreach (var file in files)
            {
                imageEntities.Add(new Image(await _imageService.SaveImageToDiskAsync(file)));
            }

            await _housingRepository.AddImages(housingId, imageEntities);
            var imageDtos = _autoMapper.Map<List<ImageDto>>(imageEntities);
            _imageService.PrepareDto(HttpContext, BrokerFileController.ImageDownloadApiEndpoint, imageDtos);

            return Ok(imageDtos);
        }
            else
            {
                var reason = result.Failure.FailureReasons.First(x => Enum.TryParse<HousingAuthorizationFailureReasons>(x.Message, false, out _));
                return BadRequest(new ErrorMessageDto(HttpStatusCode.BadRequest, reason.Message));
            }
        }

        /// <summary>
        /// An API endpoint for deleting housing objects. 
        /// </summary>
        /// <param name="id">The ID of the housing object to delete.</param>
        /// <!-- Author: Jimmie, Marcus -->
        /// <!-- Co Authors: -->
        [Authorize]
        [HttpDelete("housing/{id:int}")]
        [ProducesResponseType<HousingDto>(StatusCodes.Status200OK)]
        [ProducesResponseType<ErrorMessageDto>(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> DeleteHousing(int id)
        {
            int brokerFirmId = int.Parse(User.FindFirst(ApplicationUserClaims.BrokerFirmId)!.Value);
            var housing = await _housingRepository.GetHousingByIdAsync(id, brokerFirmId);

            if (housing == null)
            {
                return NotFound(new ErrorMessageDto(HttpStatusCode.BadRequest, "No matching housing object were found."));
            }

            var authData = new DeleteHousingAuthorizationData(existingHousingBrokerFirmId: housing.BrokerFirm.BrokerFirmId,
                existingHousingBrokerId: housing.Broker.BrokerId);
            var result = await _authorizationService.AuthorizeAsync(User, authData, ApplicationPolicies.CanDeleteHousingResource);

            if (result.Succeeded)
            {
                _imageService.DeleteImagesFromDisk((await _housingRepository.GetImages(id)).Select(x => x.FileName).ToList());
                await _housingRepository.DeleteImages(id);
                await _housingRepository.DeleteHousing(id);

                return Ok();
            }
            else
            {
                var reason = result.Failure.FailureReasons.First(x => Enum.TryParse<HousingAuthorizationFailureReasons>(x.Message, false, out _));
                return BadRequest(new ErrorMessageDto(HttpStatusCode.BadRequest, reason.Message));
            }
        }

        /// <summary>
        /// An API endpoint for deleting housing images. 
        /// </summary>
        /// <param name="id">The ID of the image to delete.</param>
        /// <param name="housingId">The ID of the housing object the image belongs to</param>
        /// <!-- Author: Jimmie, Marcus -->
        /// <!-- Co Authors: -->
        [Authorize(policy: ApplicationPolicies.Broker)]
        [HttpDelete("housing/{housingId:int}/image/{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType<ErrorMessageDto>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ErrorMessageDto>(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteHousingImage([Required] int id, [Required] int housingId)
        {
            // TODO - USer authorization service

            var brokerFirmId = int.Parse(User.FindFirst(ApplicationUserClaims.BrokerFirmId)!.Value);
            var brokerId = int.Parse(User.FindFirst(ApplicationUserClaims.BrokerId)!.Value);
            var userRole = User.FindFirst(ApplicationUserClaims.UserRole)!.Value;

            var housing = await _housingRepository.GetHousingByIdAsync(housingId);

            if (housing == null)
            {
                return NotFound(new ErrorMessageDto(HttpStatusCode.BadRequest, "The referenced housing object was not found."));
            }
            else if (!housing.Images.Any(x => x.ImageId == id))
            {
                return NotFound(new ErrorMessageDto(HttpStatusCode.BadRequest, "No image with that ID was found that belongs to the referenced housing object."));
            }
            else if (housing.BrokerFirm.BrokerFirmId != brokerFirmId)
            {
                return BadRequest(new ErrorMessageDto(HttpStatusCode.BadRequest, "The referenced housing object doesn't belong to the broker firm."));
            }
            else if (brokerId != housing.Broker.BrokerId && userRole != ApplicationUserRoles.BrokerAdmin)
            {
                return BadRequest(new ErrorMessageDto(HttpStatusCode.BadRequest, "Only administrators can modify images for housing objects that they don't manage."));
            }

            var image = await _housingRepository.GetImagebyId(housingId, id);

            if (image != null)
            {
                await _housingRepository.DeleteImage(housingId, id);
                _imageService.DeleteImageFromDisk(image.FileName);
                return Ok();
            }

            // Should never get here
            return NotFound(new ErrorMessageDto(HttpStatusCode.BadRequest, "No image with that ID was found."));
        }

        /// <summary>
        /// An API endpoint for deleting housing images. 
        /// </summary>
        /// <param name="deleteImagesDto">Contains a collection of IDs for the images to delete.</param>
        /// <!-- Author: Jimmie, Marcus -->
        /// <!-- Co Authors: -->
        [Authorize(policy: ApplicationPolicies.Broker)]
        [HttpDelete("housing/{housingId:int}/images")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType<ErrorMessageDto>(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> DeleteImages([Required] int housingId, [Required][FromBody] DeleteImagesDto deleteImagesDto)
        {
            if (housingId != deleteImagesDto.HousingId)
            {
                return BadRequest(new ErrorMessageDto(HttpStatusCode.BadRequest, "Mismatch between the housing ID in the url and the one in the payload."));
            }

            var housing = await _housingRepository.GetHousingByIdAsync(housingId);

            if (housing == null)
            {
                return NotFound(new ErrorMessageDto(HttpStatusCode.BadRequest, "The referenced housing object was not found."));
            }
            else if (housing.Images.Count(x => deleteImagesDto.ImageIds.Contains(x.ImageId)) != deleteImagesDto.ImageIds.Count)
            {
                return NotFound(new ErrorMessageDto(HttpStatusCode.BadRequest, "All images doesn't belong to the referenced housing object."));
            }

            var brokerFirmId = int.Parse(User.FindFirst(ApplicationUserClaims.BrokerFirmId)!.Value);
            var brokerId = int.Parse(User.FindFirst(ApplicationUserClaims.BrokerId)!.Value);
            var authData = new DeleteHousingImageAuthorizationData(existingHousingBrokerFirmId: housing.BrokerFirm.BrokerFirmId,
                existingHousingBrokerId: housing.Broker.BrokerId);
            var result = await _authorizationService.AuthorizeAsync(User, authData, ApplicationPolicies.CanDeleteHousingImageResource);

            if (result.Succeeded)
            {
            _imageService.DeleteImagesFromDisk((await _housingRepository.GetImages(deleteImagesDto.HousingId, deleteImagesDto.ImageIds)).Select(x => x.FileName).ToList());
            await _housingRepository.DeleteImages(deleteImagesDto.HousingId, deleteImagesDto.ImageIds);

            return Ok();
        }
            else
            {
                var reason = result.Failure.FailureReasons.First(x => Enum.TryParse<HousingAuthorizationFailureReasons>(x.Message, false, out _));
                return BadRequest(new ErrorMessageDto(HttpStatusCode.BadRequest, reason.Message));
            }
        }

        /// <summary>
        /// An API endpoint for updating housing objects. 
        /// </summary>
        /// <param name="id">The ID of the housing object to update.</param>
        /// <param name="brokerFirmId">The ID of the broker firm associated with the housing.</param>
        /// <param name="updateHousingDto">The serialized DTO object.</param>
        /// <!-- Author: Jimmie, Marcus -->
        /// <!-- Co Authors: -->
        [Authorize(policy: ApplicationPolicies.Broker)]
        [HttpPut("housing/{id:int}")]
        [ProducesResponseType<HousingDto>(StatusCodes.Status200OK)]
        [ProducesResponseType<ErrorMessageDto>(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> EditHousing([Required] int id, [FromBody] EditHousingDto updateHousingDto)
        {
            var housing = await _housingRepository.GetHousingByIdAsync(id);

            if (housing == null)
            {
                return NotFound(new ErrorMessageDto(HttpStatusCode.NotFound, "The housing object doesn't exists."));
            }
            else if (id != updateHousingDto.HousingId)
            {
                return BadRequest(new ErrorMessageDto(HttpStatusCode.BadRequest, "The housing ID in the query parameter doesn't match the ID provided in the body."));
            }

            var authData = new EditHousingAuthorizationData(existingHousingBrokerFirmId: housing.BrokerFirm.BrokerFirmId,
                existingHousingBrokerId: housing.Broker.BrokerId, newHousingBrokerId: updateHousingDto.BrokerId);
            var result = await _authorizationService.AuthorizeAsync(User, authData, ApplicationPolicies.CanEditHousingResource);

            if (result.Succeeded)
            {
                _autoMapper.Map(updateHousingDto, housing);
                await _housingRepository.UpdateAsync(housing);
                return Ok();
            }
            else
            {
                var reason = result.Failure.FailureReasons.First(x => Enum.TryParse<HousingAuthorizationFailureReasons>(x.Message, false, out _));
                return BadRequest(new ErrorMessageDto(HttpStatusCode.BadRequest, reason.Message));
            }
        }

        /// <summary>
        /// An API endpoint for fetching a housing object. 
        /// </summary>
        /// <param name="id">The ID of the housing to fetch.</param>
        /// <returns>An embedded collection of <see cref="HousingDto"/>.</returns>
        /// <!-- Author: Jimmie, Marcus -->
        /// <!-- Co Authors: -->
        [Authorize(policy: ApplicationPolicies.Broker)]
        [HttpGet("housing/{id:int}")]
        [ProducesResponseType<HousingDto>(StatusCodes.Status200OK)]
        [ProducesResponseType<ErrorMessageDto>(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<HousingDto>>> GetHousingById(int id)
        {
            var housing = await _housingRepository.GetHousingByIdAsync(id);

            if (housing == null)
            {
                return NotFound();
            }
            else if (housing.BrokerFirm.BrokerFirmId != int.Parse(User.FindFirst(ApplicationUserClaims.BrokerFirmId)!.Value))
            {
                return BadRequest(new ErrorMessageDto(HttpStatusCode.BadRequest, "The referenced housing object doesn't belong to the broker firm."));
            }

            var result = _autoMapper.Map<HousingDto>(housing);
            _imageService.PrepareDto(HttpContext, BrokerFileController.ImageDownloadApiEndpoint, result);

            return Ok(result);
        }

        /// <summary>
        /// An API endpoint for fetching all housing categories.
        /// </summary>
        /// <returns>An embedded collection of <see cref="HousingCategoryDto"/>.</returns>
        /// <!-- Author: Jimmie, Marcus -->
        /// <!-- Co Authors: -->
        [Authorize(policy: ApplicationPolicies.Broker)]
        [HttpGet("housing/categories")]
        [ProducesResponseType<HousingDto>(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<HousingCategoryDto>>> GetHousingCategories()
        {
            return Ok(_autoMapper.Map<List<HousingCategoryDto>>(await _housingRepository.GetHousingCategories()));
        }

        /// <summary>
		/// An API endpoint for retrieving housing objects count being handled by a broker.
		/// </summary>
		/// <param name="brokerId">The ID of the broker.</param>
		/// <returns>An embedded <see cref="int"/></returns>
		/// <!-- Author: Jimmie, Marcus -->
        /// <!-- Co Authors: -->
        [Authorize(policy: ApplicationPolicies.Broker)]
        [HttpGet("housings/count")]
        public async Task<ActionResult<int>> GetHousingCountByBrokerId([Required] int? brokerId = null)
        {
            var brokerFirmId = int.Parse(User.FindFirst(ApplicationUserClaims.BrokerFirmId)!.Value);

            if (brokerId != null && !await _brokerFirmRepository.HaveBroker(brokerFirmId, brokerId.Value))
            {
                return BadRequest(new ErrorMessageDto(System.Net.HttpStatusCode.BadRequest, "The referenced broker doesn't belong to the broker firm."));
            }

            int housingCount = await _housingRepository.GetHousingsCountAsync(brokerId: brokerId);

            return Ok(housingCount);
        }

        /// <summary>
        /// An API endpoint for retrieving a housing image file. 
        /// </summary>
        /// <param name="id">The ID of the image to fetch.</param>
        /// <param name="housingId">The ID of the housing object the image belongs to</param>
        /// <returns>An embedded <see cref="FileResult"/> object.</returns>
        /// <!-- Author: Jimmie, Marcus -->
        /// <!-- Co Authors: -->
        [Authorize(policy: ApplicationPolicies.Broker)]
        [HttpGet("housing/{housingId:int}/image/{id:int}")]
        [ProducesResponseType<FileContentResult>(StatusCodes.Status200OK)]
        [ProducesResponseType<ErrorMessageDto>(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetHousingImageById(int id, [Required] int housingId)
        {
            var brokerFirmId = int.Parse(User.FindFirst(ApplicationUserClaims.BrokerFirmId)!.Value);

            var housing = await _housingRepository.GetHousingByIdAsync(housingId);

            if (housing == null)
            {
                return NotFound(new ErrorMessageDto(HttpStatusCode.BadRequest, "The referenced housing object was not found."));
            }
            if (!housing.Images.Any(x => x.ImageId == id))
            {
                return BadRequest(new ErrorMessageDto(HttpStatusCode.BadRequest, "No image with that ID was found that belongs to the referenced housing object."));
            }
            else if (housing.BrokerFirm.BrokerFirmId != brokerFirmId)
            {
                return BadRequest(new ErrorMessageDto(HttpStatusCode.BadRequest, "The referenced housing object doesn't belong to the broker firm."));
            }

            var image = housing.Images.Single(x => x.ImageId == id);

            if (image != null)
            {
                var fileResult = await _imageService.PrepareImageFileDownloadAsync(image.FileName);

                if (fileResult != null)
                {
                    return fileResult;
                }
            }

            return NotFound();
        }

        /// <summary>
        /// An API endpoint for retrieving all images for a housing object. 
        /// </summary>
        /// <param name="brokerFirmId">The ID of the broker firm associated with the housing object the image belongs to.</param>
        /// <param name="housingId">The ID of the housing object the image belongs to</param>
        /// <returns>A collection of <see cref="ImageDto"/> objects if successful.</returns>
        /// <!-- Author: Jimmie, Marcus -->
        /// <!-- Co Authors: -->
        [Authorize(policy: ApplicationPolicies.Broker)]
        [HttpGet("housing/{housingId:int}/images")]
        [ProducesResponseType<List<ImageDto>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ErrorMessageDto>(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetHousingImages([Required] int housingId)
        {
            var brokerFirmId = int.Parse(User.FindFirst(ApplicationUserClaims.BrokerFirmId)!.Value);
            var housing = await _housingRepository.GetHousingByIdAsync(housingId);

            if (housing == null)
            {
                return NotFound(new ErrorMessageDto(HttpStatusCode.BadRequest, "The referenced housing object was not found."));
            }
            else if (housing.BrokerFirm.BrokerFirmId != brokerFirmId)
            {
                return BadRequest(new ErrorMessageDto(HttpStatusCode.BadRequest, "The referenced housing object doesn't belong to the broker firm."));
            }

            var images = _autoMapper.Map<List<ImageDto>>(housing.Images);

            if (images.Count > 0)
            {
                _imageService.PrepareDto(HttpContext, BrokerFileController.ImageDownloadApiEndpoint, images);

            }

            return Ok(images);
        }

        /// <summary>
        /// An API endpoint for retrieving a compressed file of all images for a housing object. 
        /// </summary>
        /// <param name="housingId">The ID of the housing object the image belongs to</param>
        /// <returns>An embedded <see cref="FileResult"/> object.</returns>
        /// <!-- Author: Jimmie, Marcus -->
        /// <!-- Co Authors: -->
        [Authorize(policy: ApplicationPolicies.Broker)]
        [HttpGet("housing/{housingId:int}/images/archive")]
        [ProducesResponseType<FileStreamResult>(StatusCodes.Status200OK, "application/zip")]
        [ProducesResponseType<ErrorMessageDto>(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetHousingImagesAsArchive([Required] int housingId)
        {
            var brokerFirmId = int.Parse(User.FindFirst(ApplicationUserClaims.BrokerFirmId)!.Value);
            var housing = await _housingRepository.GetHousingByIdAsync(housingId);

            if (housing == null)
            {
                return NotFound(new ErrorMessageDto(HttpStatusCode.BadRequest, "The referenced housing object was not found."));
            }
            else if (housing.BrokerFirm.BrokerFirmId != brokerFirmId)
            {
                return BadRequest(new ErrorMessageDto(HttpStatusCode.BadRequest, "The referenced housing object doesn't belong to the broker firm."));
            }

            var images = await _housingRepository.GetImages(housingId);

            if (images.Count > 0)
            {
                var result = await _imageService.PrepareImageFilesZipDownloadAsync(housing.Images.Select(x => x.FileName).ToList());

                if (result != null)
                {
                    return result;
                }
            }

            return NotFound();
        }

        /// <summary>
        /// An API endpoint for retrieving housing objects being handled by a brokerfirm.
        /// </summary>
        /// <param name="brokerId">Filters the housing objects after a broker.</param>
        /// <returns>An embedded <see cref="HousingDto"/> object.</returns>
        /// <!-- Author: Jimmie, Marcus -->
        /// <!-- Co Authors: -->
        [Authorize(policy: ApplicationPolicies.Broker)]
        [HttpGet("housings")]
        [ProducesResponseType<List<HousingDto>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ErrorMessageDto>(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<HousingDto>>> GetHousings(int? limitImagesPerHousing = null, int? brokerId = null)
        {
            var brokerFirmId = int.Parse(User.FindFirst(ApplicationUserClaims.BrokerFirmId)!.Value);

            if (brokerId != null && !await _brokerFirmRepository.HaveBroker(brokerFirmId, brokerId.Value))
            {
                return BadRequest(new ErrorMessageDto(System.Net.HttpStatusCode.BadRequest, "The referenced broker doesn't belong to the broker firm."));
            }

            List<HousingDto> result = _autoMapper.Map<List<HousingDto>>(await _housingRepository.GetHousingsAsync(brokerFirmId: brokerFirmId, brokerId: brokerId,
                limitImagesPerHousing: limitImagesPerHousing));
            _imageService.PrepareDto(HttpContext, BrokerFileController.ImageDownloadApiEndpoint, result);

            return Ok(result);
        }

        /// <summary>
        /// An API endpoint for fetching all municipalities.
        /// </summary>
        /// <returns>An embedded collection of <see cref="MunicipalityDto"/>.</returns>
        /// <!-- Author: Jimmie, Marcus -->
        /// <!-- Co Authors: -->
        [Authorize(policy: ApplicationPolicies.Broker)]
        [HttpGet("municipalities")]
        [ProducesResponseType<MunicipalityDto>(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<MunicipalityDto>>> GetMunicipalities()
        {
            return Ok(_autoMapper.Map<List<MunicipalityDto>>(await _housingRepository.GetMunicipalities()));
        }

        #endregion
    }
}
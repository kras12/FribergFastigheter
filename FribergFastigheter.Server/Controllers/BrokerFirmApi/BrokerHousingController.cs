using AutoMapper;
using FribergFastigheter.Server.Data.Entities;
using FribergFastigheter.Shared.Dto;
using FribergFastigheter.Server.Data.Interfaces;
using FribergFastigheter.Server.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.Json;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using FribergFastigheter.Server.Data.Constants;

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

        /// <summary>
        /// The injected Auto Mapper.
        /// </summary>
        private readonly IMapper _autoMapper;
		#endregion

		#region Constructors

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="housingRepository">The injected housing repository.</param>
		/// <param name="mapper">The injected Auto Mapper.</param>
		/// <param name="imageService">The injected imageService properties.</param>
		/// <param name="brokerFirmRepository">The injected broker firm repository.</param>
		public BrokerHousingController(IHousingRepository housingRepository, IMapper mapper, IImageService imageService, IBrokerFirmRepository brokerFirmRepository)
		{
			_housingRepository = housingRepository;
			_autoMapper = mapper;
			_imageService = imageService;
			_brokerFirmRepository = brokerFirmRepository;
		}

        #endregion

        #region ApiEndPoints

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
            if (int.Parse(User.FindFirst(ApplicationUserClaims.BrokerFirmId)!.Value) != id)
            {
                return BadRequest(new ErrorMessageDto(System.Net.HttpStatusCode.BadRequest, "Can't delete housings that belongs to another broker firm."));
            }

            int brokerFirmId = int.Parse(User.FindFirst(ApplicationUserClaims.BrokerFirmId)!.Value);
            var housing = await _housingRepository.GetHousingByIdAsync(id, brokerFirmId);

            if (housing == null)
            {
                return NotFound(new ErrorMessageDto(HttpStatusCode.BadRequest, "No matching housing object were found."));
            }

            if (User.FindFirst(ApplicationUserClaims.UserRole)!.Value == ApplicationUserRoles.Broker && housing.Broker.BrokerId != id)
            {
                return BadRequest(new ErrorMessageDto(System.Net.HttpStatusCode.BadRequest, "Can't delete a housing object that is managed by someone else."));
            }

            _imageService.DeleteImagesFromDisk((await _housingRepository.GetImages(id)).Select(x => x.FileName).ToList());
            await _housingRepository.DeleteImages(id);
            await _housingRepository.DeleteHousing(id);

            return Ok();
        }

        /// <summary>
        /// An API endpoint for fetching a housing object. 
        /// </summary>
        /// <param name="id">The ID of the housing to fetch.</param>
        /// <returns>An embedded collection of <see cref="HousingDto"/>.</returns>
        /// <!-- Author: Jimmie, Marcus -->
        /// <!-- Co Authors: -->
        [Authorize]
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
        [Authorize]
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
        [Authorize]
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
        /// An API endpoint for retrieving housing objects being handled by a brokerfirm.
        /// </summary>
        /// <param name="brokerId">Filters the housing objects after a broker.</param>
        /// <returns>An embedded <see cref="HousingDto"/> object.</returns>
        /// <!-- Author: Jimmie, Marcus -->
        /// <!-- Co Authors: -->
        [Authorize]
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
        [Authorize]
        [HttpGet("municipalities")]
        [ProducesResponseType<MunicipalityDto>(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<MunicipalityDto>>> GetMunicipalities()
        {
            return Ok(_autoMapper.Map<List<MunicipalityDto>>(await _housingRepository.GetMunicipalities()));
        }

        /// <summary>
        /// An API endpoint for creating housing objects. 
        /// </summary>
        /// <param name="newHousingDto">The serialized input data.</param>
        /// <param name="returnCreatedHousing">True to return the created housing object.</param>
        /// <!-- Author: Jimmie, Marcus -->
        /// <!-- Co Authors: -->
        [Authorize]
        [HttpPost("housings")]
		[ProducesResponseType<HousingDto>(StatusCodes.Status201Created)]
		[ProducesResponseType<ErrorMessageDto>(StatusCodes.Status400BadRequest)]        
		public async Task<ActionResult> CreateHousing([FromBody] CreateHousingDto newHousingDto, bool returnCreatedHousing)
        {
            var brokerFirmId = int.Parse(User.FindFirst(ApplicationUserClaims.BrokerFirmId)!.Value);
            var brokerId = int.Parse(User.FindFirst(ApplicationUserClaims.BrokerId)!.Value);
            var userRole = User.FindFirst(ApplicationUserClaims.UserRole)!.Value;

            if (brokerFirmId != newHousingDto.BrokerFirmId)
			{
				return BadRequest(new ErrorMessageDto(HttpStatusCode.BadRequest, "The referenced broker firm doesn't match the one in the posted housing object."));
			}

            if (brokerId != newHousingDto.BrokerId && userRole != ApplicationUserRoles.BrokerAdmin)
            {
                return BadRequest(new ErrorMessageDto(HttpStatusCode.BadRequest, "Only administrators can create a housing object for another broker."));
            }

			var newHousingEntity = _autoMapper.Map<Housing>(newHousingDto);
            await _housingRepository.AddAsync(newHousingEntity);

			if (returnCreatedHousing)
			{
                var result = _autoMapper.Map<HousingDto>(await _housingRepository.GetHousingByIdAsync(newHousingEntity.HousingId));
                _imageService.PrepareDto(HttpContext, BrokerFileController.ImageDownloadApiEndpoint, result);
                return CreatedAtAction(nameof(GetHousingById), new { id = newHousingEntity.HousingId }, result);
            }
			else
			{
                return Created();
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
        [Authorize]
        [HttpPut("housing/{id:int}")]
		[ProducesResponseType<HousingDto>(StatusCodes.Status200OK)]
		[ProducesResponseType<ErrorMessageDto>(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult> UpdateHousing([Required] int id, [FromBody] EditHousingDto updateHousingDto)
        {
            var brokerFirmId = int.Parse(User.FindFirst(ApplicationUserClaims.BrokerFirmId)!.Value);
            var brokerId = int.Parse(User.FindFirst(ApplicationUserClaims.BrokerId)!.Value);
            var userRole = User.FindFirst(ApplicationUserClaims.UserRole)!.Value;

            if (brokerFirmId != updateHousingDto.BrokerFirmId)
            {
                return BadRequest(new ErrorMessageDto(HttpStatusCode.BadRequest, "The referenced broker firm doesn't match the one in the posted housing object."));
            }

            if (brokerId != updateHousingDto.BrokerId && userRole != ApplicationUserRoles.BrokerAdmin)
            {
                return BadRequest(new ErrorMessageDto(HttpStatusCode.BadRequest, "Only administrators can update a housing object for another broker."));
            }

            if (id != updateHousingDto.HousingId || !await _housingRepository.IsOwnedByBrokerFirm(id, brokerFirmId))
            {
				return BadRequest(new ErrorMessageDto(HttpStatusCode.BadRequest, "The referenced housing object doesn't belong to the broker firm."));
			}

            var updatedHousingEntity = _autoMapper.Map<Housing>(updateHousingDto);
            await _housingRepository.UpdateAsync(updatedHousingEntity);
            return Ok();
        }

        /// <summary>
        /// An API endpoint for deleting housing images. 
        /// </summary>
        /// <param name="id">The ID of the image to delete.</param>
        /// <param name="housingId">The ID of the housing object the image belongs to</param>
        /// <!-- Author: Jimmie, Marcus -->
        /// <!-- Co Authors: -->
        [Authorize]
        [HttpDelete("housing/{housingId:int}/image/{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType<ErrorMessageDto>(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> DeleteHousingImage([Required] int id, [Required] int housingId)
        {
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
        [Authorize]
        [HttpDelete("housing/{housingId:int}/images")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType<ErrorMessageDto>(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> DeleteImages([Required] int housingId, [Required][FromBody] DeleteImagesDto deleteImagesDto)
        {
            var brokerFirmId = int.Parse(User.FindFirst(ApplicationUserClaims.BrokerFirmId)!.Value);
            var brokerId = int.Parse(User.FindFirst(ApplicationUserClaims.BrokerId)!.Value);
            var userRole = User.FindFirst(ApplicationUserClaims.UserRole)!.Value;

            var housing = await _housingRepository.GetHousingByIdAsync(housingId);

            if (housing == null)
            {
                return NotFound(new ErrorMessageDto(HttpStatusCode.BadRequest, "The referenced housing object was not found."));
            }
            else if (housing.Images.Count(x => deleteImagesDto.ImageIds.Contains(x.ImageId)) != deleteImagesDto.ImageIds.Count)
            {
                return NotFound(new ErrorMessageDto(HttpStatusCode.BadRequest, "All images doesn't belong to the referenced housing object."));
            }
            else if (housing.BrokerFirm.BrokerFirmId != brokerFirmId)
            {
                return BadRequest(new ErrorMessageDto(HttpStatusCode.BadRequest, "The referenced housing object doesn't belong to the broker firm."));
            }
            else if (brokerId != housing.Broker.BrokerId && userRole != ApplicationUserRoles.BrokerAdmin)
            {
                return BadRequest(new ErrorMessageDto(HttpStatusCode.BadRequest, "Only administrators can modify images for housing objects that they don't manage."));
            }
            else if (housingId != deleteImagesDto.HousingId)
            {
                return BadRequest(new ErrorMessageDto(HttpStatusCode.BadRequest, "Mismatch between the housing ID in the url and the one in the payload."));
            }

            _imageService.DeleteImagesFromDisk((await _housingRepository.GetImages(deleteImagesDto.HousingId, deleteImagesDto.ImageIds)).Select(x => x.FileName).ToList());
            await _housingRepository.DeleteImages(deleteImagesDto.HousingId, deleteImagesDto.ImageIds);

            return Ok();
        }

        /// <summary>
        /// An API endpoint for retrieving a housing image file. 
        /// </summary>
        /// <param name="id">The ID of the image to fetch.</param>
        /// <param name="housingId">The ID of the housing object the image belongs to</param>
        /// <returns>An embedded <see cref="FileResult"/> object.</returns>
        /// <!-- Author: Jimmie, Marcus -->
        /// <!-- Co Authors: -->
        [Authorize]
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

            var image = housing.Images.Single(x =>  x.ImageId == id);

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
        [Authorize]
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
        [Authorize]
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
        /// An API endpoint for creating images. 
        /// </summary>
        /// <param name="housingId">The ID of the housing object the image belongs to</param>
        /// <param name="files">A collection of uploaded image files.</param>
        /// <!-- Author: Jimmie, Marcus -->
        /// <!-- Co Authors: -->
        [Authorize]
        [HttpPost("housing/{housingId:int}/images")]
        [ProducesResponseType<List<ImageDto>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ErrorMessageDto>(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> CreateHousingImages([Required] int housingId, [FromForm] IFormFileCollection files)
        {
            var brokerFirmId = int.Parse(User.FindFirst(ApplicationUserClaims.BrokerFirmId)!.Value);
            var brokerId = int.Parse(User.FindFirst(ApplicationUserClaims.BrokerId)!.Value);
            var userRole = User.FindFirst(ApplicationUserClaims.UserRole)!.Value;

            var housing = await _housingRepository.GetHousingByIdAsync(housingId);

            if (housing == null)
            {
                return NotFound(new ErrorMessageDto(HttpStatusCode.BadRequest, "The referenced housing object was not found."));
            }
            else if (housing.BrokerFirm.BrokerFirmId != brokerFirmId)
            {
                return BadRequest(new ErrorMessageDto(HttpStatusCode.BadRequest, "The referenced housing object doesn't belong to the broker firm."));
            }
            else if (brokerId != housing.Broker.BrokerId && userRole != ApplicationUserRoles.BrokerAdmin)
            {
                return BadRequest(new ErrorMessageDto(HttpStatusCode.BadRequest, "Only administrators can modify images for housing objects that they don't manage."));
            }    
            else if (files.Count == 0)
            {
                return BadRequest(new ErrorMessageDto(HttpStatusCode.BadRequest, "No files were submitted."));
            }

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

        #endregion
    }
}
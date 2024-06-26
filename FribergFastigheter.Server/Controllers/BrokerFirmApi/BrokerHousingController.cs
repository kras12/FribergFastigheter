﻿using AutoMapper;
using FribergFastigheter.Server.Data.Entities;
using FribergFastigheter.Server.Data.Interfaces;
using FribergFastigheter.Server.Services;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using FribergFastigheter.Shared.Constants;
using FribergFastigheter.Shared.Dto.Housing;
using FribergFastigheter.Shared.Dto.Image;
using FribergFastigheter.Shared.Enums;
using FribergFastigheter.Shared.Services.AuthorizationHandlers.Housing.Data;
using FribergFastigheter.Server.Dto;
using FribergFastigheter.Shared.Dto.Api;

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
        /// The injected broker repository.
        /// </summary>
        private readonly IBrokerRepository _brokerRepository;

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
        /// <param name="brokerRepository">The injected broker repository service.</param>
        public BrokerHousingController(IHousingRepository housingRepository, IMapper mapper, IImageService imageService, 
            IBrokerFirmRepository brokerFirmRepository, IAuthorizationService authorizationService, IBrokerRepository brokerRepository)
        {
            _housingRepository = housingRepository;
            _autoMapper = mapper;
            _imageService = imageService;
            _brokerFirmRepository = brokerFirmRepository;
            _authorizationService = authorizationService;
            _brokerRepository = brokerRepository;
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
        [ProducesResponseType<MvcApiValueResponseDto<HousingDto>>(StatusCodes.Status201Created)]
        [ProducesResponseType<MvcApiErrorResponseDto>(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> CreateHousing([FromBody] CreateHousingDto newHousingDto)
        {
            var brokerFirmId = int.Parse(User.FindFirst(ApplicationUserClaims.BrokerFirmId)!.Value);
            var authData = new CreateHousingAuthorizationData(newHousingBrokerId: newHousingDto.BrokerId);
            var result = await _authorizationService.AuthorizeAsync(User, authData, ApplicationPolicies.CanCreateHousingResource);

            if (result.Succeeded)
            {
                var newHousingEntity = _autoMapper.Map<Housing>(newHousingDto);
                await _housingRepository.AddAsync(newHousingEntity);
                var finalHousingDto = _autoMapper.Map<HousingDto>(await _housingRepository.GetHousingByIdAsync(newHousingEntity.HousingId));
                _imageService.PrepareDto(HttpContext, BrokerFileController.ImageDownloadApiEndpoint, finalHousingDto);

                return CreatedAtAction(nameof(GetHousingById), new { id = newHousingEntity.HousingId }, new MvcApiValueResponseDto<HousingDto>(finalHousingDto));
            }
            else
            {
                return Unauthorized(new MvcApiErrorResponseDto(result));
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
        [ProducesResponseType<MvcApiValueResponseDto<List<ImageDto>>>(StatusCodes.Status200OK)]
        [ProducesResponseType<MvcApiErrorResponseDto>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<MvcApiErrorResponseDto>(StatusCodes.Status404NotFound)]
        [ProducesResponseType<MvcApiErrorResponseDto>(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> CreateHousingImages([Required] int housingId, [FromForm] IFormFileCollection files)
        {
            if (files.Count == 0)
            {
                return BadRequest(new MvcApiErrorResponseDto(ApiErrorMessageTypes.IncompleteInputData, "No files were submitted."));
            }

            var housing = await _housingRepository.GetHousingByIdAsync(housingId);

            if (housing == null)
            {
                return NotFound(new MvcApiErrorResponseDto(ApiErrorMessageTypes.ResourceNotFound, "The referenced housing object was not found."));
            }

            var authData = new CreateHousingImageAuthorizationData(existingHousingBrokerFirmId: housing.Broker.BrokerFirm.BrokerFirmId,
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

                return Ok(new MvcApiValueResponseDto<List<ImageDto>>(imageDtos));
            }
            else
            {
                return Unauthorized(new MvcApiErrorResponseDto(result));
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
        [ProducesResponseType<MvcApiEmptyResponseDto>(StatusCodes.Status200OK)]
        [ProducesResponseType<MvcApiErrorResponseDto>(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType<MvcApiErrorResponseDto>(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteHousing(int id)
        {
            int brokerFirmId = int.Parse(User.FindFirst(ApplicationUserClaims.BrokerFirmId)!.Value);
            var housing = await _housingRepository.GetHousingByIdAsync(id, brokerFirmId);

            if (housing == null)
            {
                return NotFound(new MvcApiErrorResponseDto(ApiErrorMessageTypes.ResourceNotFound, "No matching housing object were found."));
            }

            var authData = new DeleteHousingAuthorizationData(existingHousingBrokerFirmId: housing.Broker.BrokerFirm.BrokerFirmId,
                existingHousingBrokerId: housing.Broker.BrokerId);
            var result = await _authorizationService.AuthorizeAsync(User, authData, ApplicationPolicies.CanDeleteHousingResource);

            if (result.Succeeded)
            {
                _imageService.DeleteImagesFromDisk((await _housingRepository.GetImages(id)).Select(x => x.FileName).ToList());
                await _housingRepository.DeleteImages(id);
                housing.IsDeleted = true;
                await _housingRepository.UpdateAsync(housing);

                return Ok(new MvcApiEmptyResponseDto());
            }
            else
            {
                return Unauthorized(new MvcApiErrorResponseDto(result));
            }
        }        

        /// <summary>
        /// An API endpoint for deleting housing images. 
        /// </summary>
        /// <param name="deleteImagesDto">Contains a collection of IDs for the images to delete.</param>
        /// <!-- Author: Jimmie, Marcus -->
        /// <!-- Co Authors: -->
        [Authorize(policy: ApplicationPolicies.Broker)]
        [HttpDelete("housing/{housingId:int}/images")]
        [ProducesResponseType<MvcApiEmptyResponseDto>(StatusCodes.Status200OK)]
        [ProducesResponseType<MvcApiErrorResponseDto>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<MvcApiErrorResponseDto>(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> DeleteImages([Required] int housingId, [Required][FromBody] DeleteImagesDto deleteImagesDto)
        {
            if (housingId != deleteImagesDto.HousingId)
            {
                return BadRequest(new MvcApiErrorResponseDto(ApiErrorMessageTypes.InputDataConflict, "Mismatch between the housing ID in the url and the one in the payload."));
            }

            var housing = await _housingRepository.GetHousingByIdAsync(housingId);

            if (housing == null)
            {
                return BadRequest(new MvcApiErrorResponseDto(ApiErrorMessageTypes.ResourceNotFound, "The referenced housing object was not found."));
            }
            else if (housing.Images.Count(x => deleteImagesDto.ImageIds.Contains(x.ImageId)) != deleteImagesDto.ImageIds.Count)
            {
                return BadRequest(new MvcApiErrorResponseDto(ApiErrorMessageTypes.ResourceOwnershipConflict, "All images doesn't belong to the referenced housing object."));
            }

            var brokerFirmId = int.Parse(User.FindFirst(ApplicationUserClaims.BrokerFirmId)!.Value);
            var brokerId = int.Parse(User.FindFirst(ApplicationUserClaims.BrokerId)!.Value);
            var authData = new DeleteHousingImageAuthorizationData(existingHousingBrokerFirmId: housing.Broker.BrokerFirm.BrokerFirmId,
                existingHousingBrokerId: housing.Broker.BrokerId);
            var result = await _authorizationService.AuthorizeAsync(User, authData, ApplicationPolicies.CanDeleteHousingImageResource);

            if (result.Succeeded)
            {
                _imageService.DeleteImagesFromDisk((await _housingRepository.GetImages(deleteImagesDto.HousingId, deleteImagesDto.ImageIds)).Select(x => x.FileName).ToList());
                await _housingRepository.DeleteImages(deleteImagesDto.HousingId, deleteImagesDto.ImageIds);

                return Ok(new MvcApiEmptyResponseDto());
            }
            else
            {
                return Unauthorized(new MvcApiErrorResponseDto(result));
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
        [ProducesResponseType<MvcApiValueResponseDto<HousingDto>>(StatusCodes.Status200OK)]
        [ProducesResponseType<MvcApiErrorResponseDto>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<MvcApiErrorResponseDto>(StatusCodes.Status404NotFound)]
        [ProducesResponseType<MvcApiErrorResponseDto>(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> EditHousing([Required] int id, [FromBody] EditHousingDto updateHousingDto)
        {
            var housing = await _housingRepository.GetHousingByIdAsync(id);

            if (housing == null)
            {
                return NotFound(new MvcApiErrorResponseDto(ApiErrorMessageTypes.ResourceNotFound, "The housing object doesn't exists."));
            }
            else if (id != updateHousingDto.HousingId)
            {
                return BadRequest(new MvcApiErrorResponseDto(ApiErrorMessageTypes.InputDataConflict, "The housing ID in the query parameter doesn't match the ID provided in the body."));
            }

            var authData = new EditHousingAuthorizationData(existingHousing: _autoMapper.Map<HousingDto>(housing), newHousing: updateHousingDto);
            var result = await _authorizationService.AuthorizeAsync(User, authData, ApplicationPolicies.CanEditHousingResource);

            if (result.Succeeded)
            {
                _autoMapper.Map(updateHousingDto, housing);
                await _housingRepository.UpdateAsync(housing);

                var updatedHousingDto = _autoMapper.Map<HousingDto>(await _housingRepository.GetHousingByIdAsync(housing.HousingId));
                _imageService.PrepareDto(HttpContext, BrokerFileController.ImageDownloadApiEndpoint, updatedHousingDto);

                return Ok(new MvcApiValueResponseDto<HousingDto>(updatedHousingDto));
            }
            else
            {
                return Unauthorized(new MvcApiErrorResponseDto(result));
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
        [ProducesResponseType<MvcApiValueResponseDto<HousingDto>>(StatusCodes.Status200OK)]
        [ProducesResponseType<MvcApiErrorResponseDto>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<MvcApiErrorResponseDto>(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<HousingDto>>> GetHousingById(int id)
        {
            var housing = await _housingRepository.GetHousingByIdAsync(id);

            if (housing == null)
            {
                return NotFound(new MvcApiErrorResponseDto(ApiErrorMessageTypes.ResourceNotFound, "The housing object does not exists."));
            }

            var authData = new BrokerFirmAssociationAuthorizationData(housing.Broker.BrokerFirm.BrokerFirmId);
            var result = await _authorizationService.AuthorizeAsync(User, authData, ApplicationPolicies.BrokerFirmAssociation);

            if (result.Succeeded)
            {
                var housingDto = _autoMapper.Map<HousingDto>(housing);
                _imageService.PrepareDto(HttpContext, BrokerFileController.ImageDownloadApiEndpoint, housingDto);

                return Ok(new MvcApiValueResponseDto<HousingDto>(housingDto));
            }
            else
            {
                return Unauthorized(new MvcApiErrorResponseDto(result));
            }
        }

        /// <summary>
        /// An API endpoint for fetching all housing categories.
        /// </summary>
        /// <returns>An embedded collection of <see cref="HousingCategoryDto"/>.</returns>
        /// <!-- Author: Jimmie, Marcus -->
        /// <!-- Co Authors: -->
        [Authorize(policy: ApplicationPolicies.Broker)]
        [HttpGet("housing/categories")]
        [ProducesResponseType<MvcApiValueResponseDto<List<HousingCategoryDto>>>(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<HousingCategoryDto>>> GetHousingCategories()
        {
            return Ok(new MvcApiValueResponseDto<List<HousingCategoryDto>>(_autoMapper.Map<List<HousingCategoryDto>>(await _housingRepository.GetHousingCategories())));
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
        [ProducesResponseType<MvcApiValueResponseDto<MvcApiValueResponseDto<object>>>(StatusCodes.Status200OK)]
        [ProducesResponseType<MvcApiErrorResponseDto>(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<int>> GetHousingCount([Required] int? brokerId = null)
        {
            if (brokerId != null)
            {
                var broker = await _brokerRepository.GetBrokerByIdAsync(brokerId.Value);

                if (broker == null)
                {
                    return BadRequest(new MvcApiErrorResponseDto(ApiErrorMessageTypes.ResourceNotFound, "The broker doesn't exists."));
                }

                var authData = new BrokerFirmAssociationAuthorizationData(broker.BrokerFirm.BrokerFirmId);
                var result = await _authorizationService.AuthorizeAsync(User, authData, ApplicationPolicies.BrokerFirmAssociation);

                if (!result.Succeeded)
                {
                    return Unauthorized(new MvcApiErrorResponseDto(result));
                }
            }

            int brokerFirmId = int.Parse(User.FindFirst(ApplicationUserClaims.BrokerFirmId)!.Value);
            int housingCount = await _housingRepository.GetHousingsCountAsync(brokerId: brokerId, brokerFirmId: brokerFirmId);
            return Ok(new MvcApiValueResponseDto<ApiResponseValueTypeDto<int>>(new ApiResponseValueTypeDto<int>(housingCount)));
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
        [ProducesResponseType<MvcApiErrorResponseDto>(StatusCodes.Status404NotFound)]
        [ProducesResponseType<MvcApiErrorResponseDto>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<MvcApiErrorResponseDto>(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetHousingImageById(int id, [Required] int housingId)
        {
            var housing = await _housingRepository.GetHousingByIdAsync(housingId);

            if (housing == null)
            {
                return BadRequest(new MvcApiErrorResponseDto(ApiErrorMessageTypes.ResourceNotFound, "The referenced housing object was not found."));
            }

            var authData = new BrokerFirmAssociationAuthorizationData(housing.Broker.BrokerFirm.BrokerFirmId);
            var result = await _authorizationService.AuthorizeAsync(User, authData, ApplicationPolicies.BrokerFirmAssociation);

            if (result.Succeeded)
            {
                var image = housing.Images.Single(x => x.ImageId == id);

                if (image == null)
                {
                    return NotFound(new MvcApiErrorResponseDto(ApiErrorMessageTypes.ResourceNotFound, "The image was not found."));
                }

                if (image != null)
                {
                    var fileResult = await _imageService.PrepareImageFileDownloadAsync(image.FileName);

                    if (fileResult != null)
                    {
                        return fileResult;
                    }
                }

                return NotFound(new MvcApiErrorResponseDto(ApiErrorMessageTypes.ResourceNotFound, "The image doesn't exists."));
            }
            else
            {
                return Unauthorized(new MvcApiErrorResponseDto(result));
            }            
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
        [ProducesResponseType<List<MvcApiValueResponseDto<List<ImageDto>>>>(StatusCodes.Status200OK)]
        [ProducesResponseType<MvcApiErrorResponseDto>(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetHousingImages([Required] int housingId)
        {            
            var housing = await _housingRepository.GetHousingByIdAsync(housingId);

            if (housing == null)
            {
                return BadRequest(new MvcApiErrorResponseDto(ApiErrorMessageTypes.ResourceNotFound, "The referenced housing object does not exists."));
            }

            var authData = new BrokerFirmAssociationAuthorizationData(housing.Broker.BrokerFirm.BrokerFirmId);
            var result = await _authorizationService.AuthorizeAsync(User, authData, ApplicationPolicies.BrokerFirmAssociation);

            if (result.Succeeded)
            {
                var images = _autoMapper.Map<List<ImageDto>>(housing.Images);

                if (images.Count > 0)
                {
                    _imageService.PrepareDto(HttpContext, BrokerFileController.ImageDownloadApiEndpoint, images);

                }

                return Ok(new MvcApiValueResponseDto<List<ImageDto>>(images));
            }
            else
            {
                return Unauthorized(new MvcApiErrorResponseDto(result));
            }            
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
        [ProducesResponseType<MvcApiErrorResponseDto>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<MvcApiErrorResponseDto>(StatusCodes.Status404NotFound)]
        [ProducesResponseType<MvcApiErrorResponseDto>(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetHousingImagesAsArchive([Required] int housingId)
        {
            var housing = await _housingRepository.GetHousingByIdAsync(housingId);
            
            if (housing == null)
            {
                return BadRequest(new MvcApiErrorResponseDto(ApiErrorMessageTypes.ResourceNotFound, "The referenced housing object was not found."));
            }

            var authData = new BrokerFirmAssociationAuthorizationData(housing.Broker.BrokerFirm.BrokerFirmId);
            var result = await _authorizationService.AuthorizeAsync(User, authData, ApplicationPolicies.BrokerFirmAssociation);

            if (result.Succeeded)
            {
                var images = await _housingRepository.GetImages(housingId);

                if (images.Count > 0)
                {
                    var fileStreamResult = await _imageService.PrepareImageFilesZipDownloadAsync(housing.Images.Select(x => x.FileName).ToList());

                    if (result != null)
                    {
                        return fileStreamResult;
                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status500InternalServerError, new MvcApiErrorResponseDto(ApiErrorMessageTypes.GeneralError, "An error occured while preparing the file."));
                    }
                }
                else
                {
                    return NotFound(new MvcApiErrorResponseDto(ApiErrorMessageTypes.ResourceNotFound, "No images were found."));
                }
            }
            else
            {
                return Unauthorized(new MvcApiErrorResponseDto(result));
            }
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
        [ProducesResponseType<List<MvcApiValueResponseDto<List<HousingDto>>>>(StatusCodes.Status200OK)]
        [ProducesResponseType<MvcApiErrorResponseDto>(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<HousingDto>>> GetHousings(int? limitImagesPerHousing = null, int? brokerId = null)
        {
            if (brokerId != null)
            {
                var broker = await _brokerRepository.GetBrokerByIdAsync(brokerId.Value);

                if (broker == null)
                {
                    return BadRequest(new MvcApiErrorResponseDto(ApiErrorMessageTypes.ResourceNotFound, "The broker doesn't exists."));
                }

                var authData = new BrokerFirmAssociationAuthorizationData(broker.BrokerFirm.BrokerFirmId);
                var authorizationResult = await _authorizationService.AuthorizeAsync(User, authData, ApplicationPolicies.BrokerFirmAssociation);

                if (!authorizationResult.Succeeded)
                {
                    return Unauthorized(new MvcApiErrorResponseDto(authorizationResult));
                }
            }

            var brokerFirmId = int.Parse(User.FindFirst(ApplicationUserClaims.BrokerFirmId)!.Value);
            List<HousingDto> result = _autoMapper.Map<List<HousingDto>>(await _housingRepository.GetHousingsAsync(brokerFirmId: brokerFirmId, brokerId: brokerId,
                limitImagesPerHousing: limitImagesPerHousing));
            _imageService.PrepareDto(HttpContext, BrokerFileController.ImageDownloadApiEndpoint, result);

            return Ok(new MvcApiValueResponseDto<List<HousingDto>>(result));

        }

        /// <summary>
        /// An API endpoint for fetching all municipalities.
        /// </summary>
        /// <returns>An embedded collection of <see cref="MunicipalityDto"/>.</returns>
        /// <!-- Author: Jimmie, Marcus -->
        /// <!-- Co Authors: -->
        [Authorize(policy: ApplicationPolicies.Broker)]
        [HttpGet("municipalities")]
        [ProducesResponseType<MvcApiValueResponseDto<List<MunicipalityDto>>>(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<MunicipalityDto>>> GetMunicipalities()
        {
            return Ok(new MvcApiValueResponseDto<List<MunicipalityDto>>(_autoMapper.Map<List<MunicipalityDto>>(await _housingRepository.GetMunicipalities())));
        }

        #endregion
    }
}
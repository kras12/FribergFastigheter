using AutoMapper;
using FribergFastigheter.Data.Entities;
using FribergFastigheter.Shared.Dto;
using FribergFastigheter.Server.Data.Interfaces;
using FribergFastigheter.Server.Services;
using FribergFastigheterApi.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.Json;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.IO.Compression;
using System.Diagnostics;

namespace FribergFastigheter.Server.Controllers.BrokerFirmApi
{
    /// <summary>
    /// An API controller for the broker firm housing image API.
    /// </summary>
    /// <!-- Author: Jimmie -->
    /// <!-- Co Authors: -->
    /// 
    [Route("api/BrokerFirm/Housing/Image")]
    [ApiController]
    public class BrokerHousingImageController : ControllerBase
    {
        #region Fields

        /// <summary>
        /// The injected Auto Mapper.
        /// </summary>
        private readonly IMapper _autoMapper;

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
        /// <param name="imageService">The injected imageService properties.</param>
        /// <param name="autoMapper"The injected Auto Mapper.></param>
        public BrokerHousingImageController(IHousingRepository housingRepository, IImageService imageService, IMapper autoMapper)
        {
            _housingRepository = housingRepository;
            _imageService = imageService;
            _autoMapper = autoMapper;
        }

        #endregion

        #region ApiEndPoints

        /// <summary>
        /// An API endpoint for deleting housing images. 
        /// </summary>
        /// <param name="id">The ID of the image to delete.</param>
        /// <param name="brokerFirmId">The ID of the broker firm associated with the housing object the image belongs to.</param>
        /// <param name="housingId">The ID of the housing object the image belongs to</param>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType<ErrorMessageDto>(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Delete(int id, [Required] int brokerFirmId, [Required] int housingId)
        {
            if (!await _housingRepository.OwnsImage(housingId, id))
            {
                return NotFound(new ErrorMessageDto(HttpStatusCode.BadRequest, "No image with that ID was found that belongs to the referenced housing object."));
            }
            else if (!await _housingRepository.IsOwnedByBrokerFirm(housingId, brokerFirmId))
            {
                return BadRequest(new ErrorMessageDto(HttpStatusCode.BadRequest, "The referenced housing object doesn't belong to the broker firm."));
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
        /// <param name="brokerFirmId">The ID of the broker firm associated with the housing object the image belongs to.</param>
        /// <param name="deleteImagesDto">Contains a collection of IDs for the images to delete.</param>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType<ErrorMessageDto>(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> DeleteImages([Required] int brokerFirmId, [Required][FromBody] DeleteImagesDto deleteImagesDto)
        {
            if (!await _housingRepository.OwnsImages(deleteImagesDto.HousingId, deleteImagesDto.ImageIds))
            {
                return NotFound(new ErrorMessageDto(HttpStatusCode.BadRequest, "All images doesn't belong to the referenced housing object."));
            }
            else if (!await _housingRepository.IsOwnedByBrokerFirm(deleteImagesDto.HousingId, brokerFirmId))
            {
                return BadRequest(new ErrorMessageDto(HttpStatusCode.BadRequest, "The referenced housing object doesn't belong to the broker firm."));
            }

            _imageService.DeleteImagesFromDisk((await _housingRepository.GetImages(deleteImagesDto.HousingId, deleteImagesDto.ImageIds)).Select(x => x.FileName).ToList());
            await _housingRepository.DeleteImages(deleteImagesDto.HousingId, deleteImagesDto.ImageIds);            

            return Ok();
        }

        /// <summary>
        /// An API endpoint for retrieving a housing image file. 
        /// </summary>
        /// <param name="id">The ID of the image to fetch.</param>
        /// <param name="brokerFirmId">The ID of the broker firm associated with the housing object the image belongs to.</param>
        /// <param name="housingId">The ID of the housing object the image belongs to</param>
        /// <returns>An embedded <see cref="FileResult"/> object.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        [HttpGet("{id:int}")]
        [ProducesResponseType<FileContentResult>(StatusCodes.Status200OK)]
        [ProducesResponseType<ErrorMessageDto>(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id, [Required] int brokerFirmId, [Required] int housingId)
        {
            if (!await _housingRepository.OwnsImage(housingId, id))
            {
                return BadRequest(new ErrorMessageDto(HttpStatusCode.BadRequest, "No image with that ID was found that belongs to the referenced housing object."));
            }
            else if (!await _housingRepository.IsOwnedByBrokerFirm(housingId, brokerFirmId))
            {
                return BadRequest(new ErrorMessageDto(HttpStatusCode.BadRequest, "The referenced housing object doesn't belong to the broker firm."));
            }

            var image = await _housingRepository.GetImagebyId(housingId, id);

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
        /// An API endpoint for retrieving a housing image file. 
        /// </summary>
        /// <param name="brokerFirmId">The ID of the broker firm associated with the housing object the image belongs to.</param>
        /// <param name="housingId">The ID of the housing object the image belongs to</param>
        /// <returns>An embedded <see cref="FileResult"/> object.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        [HttpGet("Archive")]
        [ProducesResponseType<FileStreamResult>(StatusCodes.Status200OK, "application/zip")]
        [ProducesResponseType<ErrorMessageDto>(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetArchive([Required] int brokerFirmId, [Required] int housingId)
        {
            if (!await _housingRepository.IsOwnedByBrokerFirm(housingId, brokerFirmId))
            {
                return BadRequest(new ErrorMessageDto(HttpStatusCode.BadRequest, "The referenced housing object doesn't belong to the broker firm."));
            }

            var images = await _housingRepository.GetImages(housingId);

            if (images.Count > 0)
            {
                var result = await _imageService.PrepareImageFilesZipDownloadAsync(images.Select(x => x.FileName).ToList());

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
        /// <param name="brokerFirmId">The ID of the broker firm associated with the housing object the image belongs to.</param>
        /// <param name="housingId">The ID of the housing object the image belongs to</param>
        /// <param name="files">A collection of uploaded image files.</param>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        [HttpPost]
        [ProducesResponseType<List<ImageDto>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ErrorMessageDto>(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Post([Required] int brokerFirmId, [Required] int housingId, [FromForm] IFormFileCollection files)
        {
            if (!await _housingRepository.IsOwnedByBrokerFirm(housingId, brokerFirmId))
            {
                return BadRequest(new ErrorMessageDto(HttpStatusCode.BadRequest, "The referenced housing object doesn't belong to the broker firm."));
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
            _imageService.PrepareDto(HttpContext, imageDtos);

            return Ok(imageDtos);
        }		

        #endregion
    }
}
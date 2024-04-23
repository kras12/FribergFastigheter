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
    [Route("api/BrokerFirm/Broker/Image")]
    [ApiController]
    public class BrokerImageController : ControllerBase
    {
        #region Fields

        /// <summary>
        /// The injected broker repository.
        /// </summary>
        private readonly IBrokerRepository _brokerRepository;

        /// <summary>
        /// The injected imageService properties.
        /// </summary>
        private readonly IImageService _imageService;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="imageService">The injected imageService properties.</param>
        /// <param name="brokerRepository">The injected broker repository.</param>
        public BrokerImageController(IImageService imageService, IBrokerRepository brokerRepository)
        {
            _imageService = imageService;
            _brokerRepository = brokerRepository;
        }

        #endregion

        #region ApiEndPoints

        /// <summary>
        /// An API endpoint for deleting housing images. 
        /// </summary>
        /// <param name="id">The ID of the image to delete.</param>
        /// <param name="brokerFirmId">The ID of the broker firm associated with the broker object the image belongs to.</param>
        /// <param name="brokerId">The ID of the broker object the image belongs to</param>
        /// <!-- Author: Jimmie, Marcus -->
        /// <!-- Co Authors: -->
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType<ErrorMessageDto>(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Delete(int id, [Required] int brokerFirmId, [Required] int brokerId)
        {
            if (!await _brokerRepository.OwnsImage(brokerId, id))
            {
                return NotFound(new ErrorMessageDto(HttpStatusCode.BadRequest, "No image with that ID was found that belongs to the referenced broker object."));
            }
            else if (!await _brokerRepository.IsOwnedByBrokerFirm(brokerId, brokerFirmId))
            {
                return BadRequest(new ErrorMessageDto(HttpStatusCode.BadRequest, "The referenced broker object doesn't belong to the broker firm."));
            }

            var image = await _brokerRepository.GetImagebyBrokerId(brokerId);

            if (image != null)
            {
                await _brokerRepository.DeleteImage(brokerId);
                _imageService.DeleteImageFromDisk(image.FileName);
                return Ok();
            }

            // Should never get here
            return NotFound(new ErrorMessageDto(HttpStatusCode.BadRequest, "No image with that ID was found."));
        }

        /// <summary>
        /// An API endpoint for retrieving a housing image file. 
        /// </summary>
        /// <param name="id">The ID of the image to fetch.</param>
        /// <param name="brokerFirmId">The ID of the broker firm associated with the broker object the image belongs to.</param>
        /// <param name="brokerId">The ID of the broker object the image belongs to</param>
        /// <returns>An embedded <see cref="FileResult"/> object.</returns>
        /// <!-- Author: Jimmie, Marcus -->
        /// <!-- Co Authors: -->
        [HttpGet("{id:int}")]
        [ProducesResponseType<FileContentResult>(StatusCodes.Status200OK)]
        [ProducesResponseType<ErrorMessageDto>(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id, [Required] int brokerFirmId, [Required] int brokerId)
        {
            if (!await _brokerRepository.OwnsImage(brokerId, id))
            {
                return BadRequest(new ErrorMessageDto(HttpStatusCode.BadRequest, "No image with that ID was found that belongs to the referenced broker object."));
            }
            else if (!await _brokerRepository.IsOwnedByBrokerFirm(brokerId, brokerFirmId))
            {
                return BadRequest(new ErrorMessageDto(HttpStatusCode.BadRequest, "The referenced broker object doesn't belong to the broker firm."));
            }

            var image = await _brokerRepository.GetImagebyBrokerId(brokerId);

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
        /// An API endpoint for creating images. 
        /// </summary>
        /// <param name="brokerFirmId">The ID of the broker firm associated with the broker object the image belongs to.</param>
        /// <param name="brokerId">The ID of the broker object the image belongs to</param>
        /// <!-- Author: Jimmie, Marcus -->
        /// <!-- Co Authors: -->
        [HttpPost]
        [ProducesResponseType<ImageDto>(StatusCodes.Status200OK)]
        [ProducesResponseType<ErrorMessageDto>(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Post([Required] int brokerFirmId, [Required] int brokerId, IFormFile newFile)
        {
            if (!await _brokerRepository.IsOwnedByBrokerFirm(brokerId, brokerFirmId))
            {
                return BadRequest(new ErrorMessageDto(HttpStatusCode.BadRequest, "The referenced broker object doesn't belong to the broker firm."));
            }
            

            Image newImage = new Image(await _imageService.SaveImageToDiskAsync(newFile));

            await _brokerRepository.AddImage(brokerId, newImage);
            return Ok();
            
        }		

        #endregion
    }
}
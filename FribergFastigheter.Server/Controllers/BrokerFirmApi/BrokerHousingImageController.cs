﻿using AutoMapper;
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
    [Route("api/BrokerFirm/Housing/Image")]
    [ApiController]
    public class BrokerHousingImageController : ControllerBase
    {
        #region Fields

        /// <summary>
        /// The injected broker repository.
        /// </summary>
        private readonly IBrokerFirmRepository _brokerRepository;

        /// <summary>
        /// The injected broker firm repository.
        /// </summary>
        private readonly IBrokerFirmRepository _brokerFirmRepository;

        /// <summary>
        /// The injected housing repository.
        /// </summary>
        private readonly IHousingRepository _housingRepository;

        /// <summary>
        /// The injected Auto Mapper.
        /// </summary>
        private readonly IMapper _mapper;

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
        /// <param name="brokerRepository">The injected broker repository.</param>
        public BrokerHousingImageController(IHousingRepository housingRepository, IMapper mapper, IImageService imageService, 
            IBrokerFirmRepository brokerFirmRepository, IBrokerFirmRepository brokerRepository)
        {
            _housingRepository = housingRepository;
            _mapper = mapper;
            _imageService = imageService;
            _brokerFirmRepository = brokerFirmRepository;
            _brokerRepository = brokerRepository;
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
        [HttpGet]
        [ProducesResponseType<FileStreamResult>(StatusCodes.Status200OK, "application/zip")]
        [ProducesResponseType<ErrorMessageDto>(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get([Required] int brokerFirmId, [Required] int housingId)
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
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        [HttpPost]
        [ProducesResponseType<ImageDto>(StatusCodes.Status200OK)]
        [ProducesResponseType<ErrorMessageDto>(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Post([Required] int brokerFirmId, [Required] int housingId, IFormFileCollection newFiles)
        {
            if (!await _housingRepository.IsOwnedByBrokerFirm(housingId, brokerFirmId))
            {
                return BadRequest(new ErrorMessageDto(HttpStatusCode.BadRequest, "The referenced housing object doesn't belong to the broker firm."));
            }
            else if (newFiles.Count == 0)
            {
                return BadRequest(new ErrorMessageDto(HttpStatusCode.BadRequest, "No files were submitted."));
            }

            List<Image> newImages = new();

            foreach (var file in newFiles)
            {
                newImages.Add(new Image(await _imageService.SaveImageToDiskAsync(file)));
            }

            await _housingRepository.AddImages(housingId, newImages);
            return Ok();
        }		

        #endregion
    }
}
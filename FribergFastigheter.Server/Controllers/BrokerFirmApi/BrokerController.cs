using AutoMapper;
using FribergFastigheter.Data.Entities;
using FribergFastigheter.Server.Data.Interfaces;
using FribergFastigheter.Server.Data.Repositories;
using FribergFastigheter.Server.Services;
using FribergFastigheter.Shared.Dto;
using FribergFastigheterApi.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.Net;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FribergFastigheter.Server.Controllers.BrokerFirmApi
{
    /// <summary>
    /// An API controller for the brokerfirm housings API.
    /// </summary>
    /// <!-- Author: Marcus -->
    /// <!-- Co Authors: Jimmie -->
    [Route("broker-firm-api")]
    [ApiController]
    public class BrokerController : ControllerBase
    {
        #region Fields

        /// <summary>
        /// The injected imageService properties.
        /// </summary>
        private readonly IImageService _imageService;

        /// <summary>
        /// The injected brokerfirm repository.
        /// </summary>
        private readonly IBrokerFirmRepository _brokerFirmRepository;

        /// <summary>
        /// The injected housing repository.
        /// </summary>
        private readonly IBrokerRepository _brokerRepository;

        /// <summary>
        /// The injected Auto Mapper.
        /// </summary>
        private readonly IMapper _autoMapper;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="brokerRepository">The injected broker repository.</param>
        /// <param name="mapper">The injected Auto Mapper.</param>
        /// <param name="imageService">The injected imageService properties.</param>
        public BrokerController(IBrokerRepository brokerRepository, IMapper mapper, IImageService imageService, IBrokerFirmRepository brokerFirmRepository)
        {
            _brokerRepository = brokerRepository;
            _autoMapper = mapper;
            _imageService = imageService;
            _brokerFirmRepository = brokerFirmRepository;
        }

        #endregion

        #region ApiEndPoints

        /// <summary>
        /// An API endpoint for searching broker objects by brokerFirmId. 
        /// </summary>
        /// <param name="brokerFirmId">The ID of the brokerfirm associated with the broker search.</param>
        /// <returns>An embedded collection of <see cref="BrokerDto"/>.</returns>
        /// <!-- Author: Marcus -->
        /// <!-- Co Authors: Jimmie -->
        [HttpGet("brokers")]
        [ProducesResponseType<BrokerDto>(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<BrokerDto>>> GetBrokers([Required] int brokerFirmId)
        {
            if(await _brokerFirmRepository.Exists(brokerFirmId) == false)
            {
                return BadRequest(new ErrorMessageDto(HttpStatusCode.BadRequest, "The referenced brokerfirm doesn't exist."));
            }
            
            var brokers = (await _brokerRepository.GetAllBrokersByBrokerFirmIdAsync(brokerFirmId))
                .Select(x => _autoMapper.Map<BrokerDto>(x))
                .ToList();

            _imageService.PrepareDto(HttpContext, BrokerFileController.ImageDownloadApiEndpoint, brokers);
            return Ok(brokers);
        }

        /// <summary>
        /// An API endpoint for fetching a broker object. 
        /// </summary>
        /// <param name="id">The ID of the broker to fetch.</param>
        /// <param name="brokerFirmId">The ID of the brokerfirm associated with the broker search.</param>
        /// <returns>An embedded collection of <see cref="BrokerDto"/>.</returns>
        /// <!-- Author: Marcus -->
        /// <!-- Co Authors: Jimmie -->
        [HttpGet("broker/{id:int}")]
        [ProducesResponseType<BrokerDto>(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<BrokerDto>>> GetBrokerById([Required] int id, [Required] int brokerFirmId)
        {
            var broker = await _brokerRepository.GetBrokerByIdAsync(id);

            if (broker == null)
            {
                return NotFound(new ErrorMessageDto(HttpStatusCode.NotFound, $"The broker with ID '{id}' was not found."));
            }
            else if (broker.BrokerFirm.BrokerFirmId != brokerFirmId)
            {
                return BadRequest(new ErrorMessageDto(HttpStatusCode.BadRequest, "The referenced broker doesn't belong to the referenced broker firm."));
            }

            var result = _autoMapper.Map<BrokerDto>(broker);
            _imageService.PrepareDto(HttpContext, BrokerFileController.ImageDownloadApiEndpoint, result);

            return Ok(result);
        }

        /// <summary>
        /// An API endpoint for creating broker objects. 
        /// </summary>
        /// <param name="brokerFirmId">The ID of the brokerfirm associated with the creating the new broker.</param>
        /// <param name="createBrokerDto">The serialized DTO object.</param>
        /// <!-- Author: Marcus -->
        /// <!-- Co Authors: -->
        [HttpPost("brokers")]
        public async Task<ActionResult> CreateBroker([Required] int brokerFirmId, [FromBody] CreateBrokerDto newBrokerDto, bool returnCreatedBroker)
        {
            if (brokerFirmId != newBrokerDto.BrokerFirmId)
            {
                return BadRequest(new ErrorMessageDto(HttpStatusCode.BadRequest, "The referenced broker firm doesn't match the one in the posted broker object."));
            }
            var newBroker = _autoMapper.Map<Broker>(newBrokerDto);
            await _brokerRepository.AddAsync(newBroker);

            if (returnCreatedBroker)
            {
                var result = _autoMapper.Map<BrokerDto>(await _brokerRepository.GetBrokerByIdAsync(newBroker.BrokerId));
                _imageService.PrepareDto(HttpContext, BrokerFileController.ImageDownloadApiEndpoint, result);
                return CreatedAtAction(nameof(GetBrokerById), new { id = newBroker.BrokerId }, result);
            }
            else
            {
                return Created();
            }
        }

        /// <summary>
        /// An API endpoint for updating broker objects. 
        /// </summary>
        /// <param name="id">The ID of the broker associated with the update</param>
        /// <param name="brokerFirmId">The ID of the brokerfirm associated with the broker being edited.</param>
        /// <param name="editBrokerDto">The serialized DTO object.</param>
        /// /// <!-- Author: Marcus -->
        /// <!-- Co Authors: Jimmie -->
        [HttpPut("broker/{id:int}")]
        public async Task<ActionResult> UpdateBroker([Required] int id, [Required] int brokerFirmId, [FromBody] EditBrokerDto editBrokerDto)
        {
            if (id != editBrokerDto.BrokerId)
            {
				return BadRequest(new ErrorMessageDto(HttpStatusCode.BadRequest, "The referenced broker doesn't match the supplied broker object."));
			}
            else if (! await _brokerRepository.IsOwnedByBrokerFirm(id, brokerFirmId))
            {
                return BadRequest(new ErrorMessageDto(HttpStatusCode.BadRequest, "The referenced broker doesn't belong to the referenced broker firm object."));
            }

			var existingBroker = await _brokerRepository.GetBrokerByIdAsync(id);

            if (existingBroker == null)
            {
				return NotFound(new ErrorMessageDto(HttpStatusCode.NotFound, $"The broker with ID '{id}' was not found."));
			}

			var broker = _autoMapper.Map<Broker>(editBrokerDto);
            await _brokerRepository.UpdateAsync(broker);
            return Ok();
        }

        /// <summary>
        /// An API endpoint for deleting broker objects. 
        /// </summary>
        /// <param name="id">The ID of the broker object to delete.</param>
        /// <param name="brokerFirmId">The ID of the brokerfirm associated with the broker being edited.</param>
        /// <!-- Author: Marcus, Jimmie -->
        /// <!-- Co Authors: -->
        /// TODO: The delete a broker object does not work because of conflict with the housing object that have said broker as a property. Maybe a check and send a suitable response.
        [HttpDelete("broker/{id}")]
        public async Task<ActionResult> DeleteBroker(int id, [Required] int brokerFirmId)
        {
            if (!await _brokerRepository.IsOwnedByBrokerFirm(id, brokerFirmId))
            {
                return BadRequest(new ErrorMessageDto(HttpStatusCode.BadRequest, "The referenced broker doesn't belong to the referenced broker firm."));
            }

            await _brokerRepository.DeleteAsync(id);
            return Ok();
        }

        /// <summary>
        /// An API endpoint for deleting housing images. 
        /// </summary>
        /// <param name="id">The ID of the broker object the image belongs to</param>
        /// <param name="brokerFirmId">The ID of the broker firm associated with the broker object the image belongs to.</param>
        /// <!-- Author: Jimmie, Marcus -->
        /// <!-- Co Authors: -->
        [HttpDelete("broker/{id:int}/profile-image")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType<ErrorMessageDto>(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> DeleteProfileImage([Required] int id, [Required] int brokerFirmId)
        {
            if (!await _brokerRepository.IsOwnedByBrokerFirm(id, brokerFirmId))
            {
                return BadRequest(new ErrorMessageDto(HttpStatusCode.BadRequest, "The referenced broker object doesn't belong to the broker firm."));
            }

            var image = await _brokerRepository.GetProfileImage(id);

            if (image != null)
            {
                await _brokerRepository.DeleteProfileImage(id);
                _imageService.DeleteImageFromDisk(image.FileName);
                return Ok();
            }

            // Should never get here
            return NotFound(new ErrorMessageDto(HttpStatusCode.BadRequest, "No image with that ID was found."));
        }        

        /// <summary>
        /// An API endpoint for creating images. 
        /// </summary>
        /// <param name="brokerFirmId">The ID of the broker firm associated with the broker object the image belongs to.</param>
        /// <param name="id">The ID of the broker object the image belongs to</param>
        /// <!-- Author: Jimmie, Marcus -->
        /// <!-- Co Authors: -->
        [HttpPost("broker/{id:int}/profile-image")]
        [ProducesResponseType<ImageDto>(StatusCodes.Status200OK)]
        [ProducesResponseType<ErrorMessageDto>(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> CreateProfileImage([Required] int id, [Required] int brokerFirmId, [FromForm] IFormFile file)
        {
            if (!await _brokerRepository.IsOwnedByBrokerFirm(id, brokerFirmId))
            {
                return BadRequest(new ErrorMessageDto(HttpStatusCode.BadRequest, "The referenced broker object doesn't belong to the broker firm."));
            }

            Image imageEntity = new(await _imageService.SaveImageToDiskAsync(file));
            await _brokerRepository.AddImage(id, imageEntity);
            var imageDto = _autoMapper.Map<ImageDto>(imageEntity);
            _imageService.PrepareDto(HttpContext, BrokerFileController.ImageDownloadApiEndpoint, imageDto);

            return Ok(imageDto);
        }

        #endregion

    }
}


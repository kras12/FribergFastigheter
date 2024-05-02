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
    /// <!-- Co Authors: -->
    [Route("api/BrokerFirm/Broker")]
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
        private readonly IMapper _mapper;

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
            _mapper = mapper;
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
        /// <!-- Co Authors: -->
        [HttpGet]
        [ProducesResponseType<BrokerDto>(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<BrokerDto>>> Get([Required] int brokerFirmId)
        {
            if(await _brokerFirmRepository.Exists(brokerFirmId) == false)
            {
                return BadRequest(new ErrorMessageDto(HttpStatusCode.BadRequest, "The referenced brokerfirm doesn't exist."));
            }
            
            var brokers = (await _brokerRepository.GetAllBrokersByBrokerFirmIdAsync(brokerFirmId))
                .Select(x => _mapper.Map<BrokerDto>(x))
                .ToList();

            _imageService.PrepareDto(HttpContext, brokers);
            return Ok(brokers);
        }

        /// <summary>
        /// An API endpoint for fetching a broker object. 
        /// </summary>
        /// <param name="brokerId">The ID of the broker to fetch.</param>
        /// <param name="brokerFirmId">The ID of the brokerfirm associated with the broker search.</param>
        /// <returns>An embedded collection of <see cref="BrokerDto"/>.</returns>
        /// <!-- Author: Marcus -->
        /// <!-- Co Authors: Jimmie -->
        [HttpGet("{brokerId:int}")]
        [ProducesResponseType<BrokerDto>(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<BrokerDto>>> GetById([Required] int brokerId, [Required] int brokerFirmId)
        {
            var broker = await _brokerRepository.GetBrokerByIdAsync(brokerId);

            if (broker == null)
            {
                return NotFound(new ErrorMessageDto(HttpStatusCode.NotFound, $"The broker with ID '{brokerId}' was not found."));
            }
            else if (broker.BrokerFirm.BrokerFirmId != brokerFirmId)
            {
                return BadRequest(new ErrorMessageDto(HttpStatusCode.BadRequest, "The referenced broker doesn't belong to the referenced broker firm."));
            }

            var result = _mapper.Map<BrokerDto>(broker);
            _imageService.PrepareDto(HttpContext, result);

            return Ok(result);
        }

        /// <summary>
        /// An API endpoint for creating broker objects. 
        /// </summary>
        /// <param name="brokerFirmId">The ID of the brokerfirm associated with the creating the new broker.</param>
        /// <param name="createBrokerDto">The serialized DTO object.</param>
        /// <!-- Author: Marcus -->
        /// <!-- Co Authors: -->
        [HttpPost]
        public async Task<ActionResult> Post([Required] int brokerFirmId, [FromBody] CreateBrokerDto newBrokerDto, bool returnCreatedBroker)
        {
            if (brokerFirmId != newBrokerDto.BrokerFirmId)
            {
                return BadRequest(new ErrorMessageDto(HttpStatusCode.BadRequest, "The referenced broker firm doesn't match the one in the posted broker object."));
            }
            var newBroker = _mapper.Map<Broker>(newBrokerDto);
            await _brokerRepository.AddAsync(newBroker);

            if (returnCreatedBroker)
            {
                var result = _mapper.Map<BrokerDto>(await _brokerRepository.GetBrokerByIdAsync(newBroker.BrokerId));
                _imageService.PrepareDto(HttpContext, result);
                return CreatedAtAction(nameof(GetById), new { brokerId = newBroker.BrokerId }, result);
            }
            else
            {
                return Created();
            }
        }

        /// <summary>
        /// An API endpoint for updating broker objects. 
        /// </summary>
        /// <param name="brokerId">The ID of the broker associated with the update</param>
        /// <param name="brokerFirmId">The ID of the brokerfirm associated with the broker being edited.</param>
        /// <param name="editBrokerDto">The serialized DTO object.</param>
        /// /// <!-- Author: Marcus -->
        /// <!-- Co Authors: -->
        [HttpPut("{brokerId:int}")]
        public async Task<ActionResult> Put([Required] int brokerId, [Required] int brokerFirmId, [FromBody] EditBrokerDto editBrokerDto)
        {
            if (brokerId != editBrokerDto.BrokerId)
            {
				return BadRequest(new ErrorMessageDto(HttpStatusCode.BadRequest, "The referenced broker doesn't match the supplied broker object."));
			}
            else if (! await _brokerRepository.IsOwnedByBrokerFirm(brokerId, brokerFirmId))
            {
                return BadRequest(new ErrorMessageDto(HttpStatusCode.BadRequest, "The referenced broker doesn't belong to the referenced broker firm object."));
            }

			var existingBroker = await _brokerRepository.GetBrokerByIdAsync(brokerId);

            if (existingBroker == null)
            {
				return NotFound(new ErrorMessageDto(HttpStatusCode.NotFound, $"The broker with ID '{brokerId}' was not found."));
			}

			var broker = _mapper.Map<Broker>(editBrokerDto);
            await _brokerRepository.UpdateAsync(broker);
            return Ok();
        }

        /// <summary>
        /// An API endpoint for deleting broker objects. 
        /// </summary>
        /// <param name="id">The ID of the broker object to delete.</param>
        /// <!-- Author: Marcus -->
        /// <!-- Co Authors: -->
        /// TODO: The delete a broker object does not work because of conflict with the housing object that have said broker as a property. Maybe a check and send a suitable response.
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _brokerRepository.DeleteAsync(id);
            return Ok();
        }

        #endregion

    
    }

}



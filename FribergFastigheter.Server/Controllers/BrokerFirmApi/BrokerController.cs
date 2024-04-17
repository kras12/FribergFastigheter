using AutoMapper;
using FribergFastigheter.Data.Entities;
using FribergFastigheter.Server.Data.DTO;
using FribergFastigheter.Server.Data.Interfaces;
using FribergFastigheter.Server.Data.Repositories;
using FribergFastigheterApi.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FribergFastigheter.Server.Controllers.BrokerFirmApi
{
    [Route("api/BrokerFirm/Broker")]
    [ApiController]
    public class BrokerController : ControllerBase
    {
        /// <summary>
        /// An API controller for the brokerfirm housings API.
        /// </summary>
        /// <!-- Author: Marcus -->
        /// <!-- Co Authors: -->
        /// 
        #region Fields

        /// <summary>
        /// The injected configuration properties.
        /// </summary>
        private readonly IConfiguration _configuration;

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
        /// <param name="configuration">The injected configuration properties.</param>
        public BrokerController(IBrokerRepository brokerRepository, IMapper mapper, IConfiguration configuration)
        {
            _brokerRepository = brokerRepository;
            _mapper = mapper;
            _configuration = configuration;
        }

        #endregion

        // GET: api/<BrokerFirmHousing>
        [HttpGet]
		[ProducesResponseType<HousingDto>(StatusCodes.Status200OK)]
		public async Task<ActionResult<IEnumerable<BrokerDto>>> Get([Required] int brokerFirmId)
        {
            return Ok();
        }

        // GET api/<BrokerFirmHousing>/5
        [HttpGet("{id}")]
		[ProducesResponseType<HousingDto>(StatusCodes.Status200OK)]
		[ProducesResponseType<ErrorMessageDto>(StatusCodes.Status404NotFound)]
		public string GetById(int id)
        {
            // Merge code
			//if (broker == null)
			//{
			//	return NotFound(new ErrorMessageDto(System.Net.HttpStatusCode.NotFound, $"The broker with ID '{id}' was not found."));
			//}

			return "value";
        }

        // POST api/<BrokerFirmHousing>
        [HttpPost]
		[ProducesResponseType<HousingDto>(StatusCodes.Status200OK)]
		[ProducesResponseType<ErrorMessageDto>(StatusCodes.Status400BadRequest)]
		public void Post([FromBody] string value)
        {
			// Merge code
			//if (brokerFirmId != brokerDto.BrokerFirmId)
			//{
			//	return BadRequest(new ErrorMessageDto(HttpStatusCode.BadRequest, "The referenced broker firm doesn't match the one in the posted broker object."));
			//}
		}

		// PUT api/<BrokerFirmHousing>/5
		[HttpPut("{id}")]
		[ProducesResponseType<HousingDto>(StatusCodes.Status200OK)]
		[ProducesResponseType<ErrorMessageDto>(StatusCodes.Status400BadRequest)]
		public void Put(int id, [FromBody] string value)
        {
			// Merge code
			//if (brokerFirmId != brokerDto.BrokerFirmId)
			//{
			//	return BadRequest(new ErrorMessageDto(HttpStatusCode.BadRequest, "The referenced broker firm doesn't match the one in the posted broker object."));
			//}
		}

		// DELETE api/<BrokerFirmHousing>/5
		[HttpDelete("{id}")]
		[ProducesResponseType<HousingDto>(StatusCodes.Status200OK)]
		[ProducesResponseType<ErrorMessageDto>(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult> Delete(int id)
        {
            //if (!await _brokerRepository.Exists(id))
            //{
            //	return NotFound(new ErrorMessageDto(HttpStatusCode.BadRequest, "The referenced broker object doesn't exists."));
            //}
            //else if (!await _brokerRepository.IsOwnedByBrokerFirm(id, brokerFirmId))
            //{
            //	return BadRequest(new ErrorMessageDto(HttpStatusCode.BadRequest, "The referenced broker object doesn't belong to the broker firm."));
            //}

            return Ok();
		}
    }
}
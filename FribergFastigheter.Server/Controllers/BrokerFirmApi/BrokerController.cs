using AutoMapper;
using FribergFastigheter.Data.Entities;
using FribergFastigheter.Server.Data.DTO;
using FribergFastigheter.Server.Data.Interfaces;
using FribergFastigheterApi.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

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
        private readonly IBrokerFirmRepository _brokerFirmRepository;

        /// <summary>
        /// The injected Auto Mapper.
        /// </summary>
        private readonly IMapper _mapper;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="brokerFirmRepository">The injected housing repository.</param>
        /// <param name="mapper">The injected Auto Mapper.</param>
        /// <param name="configuration">The injected configuration properties.</param>
        public BrokerController(IBrokerFirmRepository brokerFirmRepository, IMapper mapper, IConfiguration configuration)
        {
            _brokerFirmRepository = brokerFirmRepository;
            _mapper = mapper;
            _configuration = configuration;
        }

        #endregion

        // GET: api/<BrokerFirmHousing>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BrokerDto>>> Get([Required] int brokerFirmId)
        {
            //var housings = (await _brokerFirmRepository.;
            return Ok();
        }

        // GET api/<BrokerFirmHousing>/5
        [HttpGet("{id}")]
        public string GetById(int id)
        {
            return "value";
        }

        // POST api/<BrokerFirmHousing>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<BrokerFirmHousing>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<BrokerFirmHousing>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
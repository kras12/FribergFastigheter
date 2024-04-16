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
    /// <summary>
    /// An API controller for the broker search housings API.
    /// </summary>
    /// <!-- Author: Jimmie -->
    /// <!-- Co Authors: -->
    [Route("api/BrokerFirm/Housing")]
    [ApiController]
    public class BrokerHousingController : ControllerBase
    {
        #region Fields

        /// <summary>
        /// The injected configuration properties.
        /// </summary>
        private readonly IConfiguration _configuration;

        /// <summary>
        /// The injected housing repository.
        /// </summary>
        private readonly IHousingRepository _housingRepository;

        /// <summary>
        /// The injected Auto Mapper.
        /// </summary>
        private readonly IMapper _mapper;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="housingRepository">The injected housing repository.</param>
        /// <param name="mapper">The injected Auto Mapper.</param>
        /// <param name="configuration">The injected configuration properties.</param>
        public BrokerHousingController(IHousingRepository housingRepository, IMapper mapper, IConfiguration configuration)
        {
            _housingRepository = housingRepository;
            _mapper = mapper;
            _configuration = configuration;
        }

        #endregion

        #region ApiEndPoints

        /// <summary>
        /// An API endpoint for deleting housing objects. 
        /// </summary>
        /// <param name="id">The ID of the housing object to delete.</param>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _housingRepository.DeleteAsync(id);
            return Ok();
        }

        /// <summary>
        /// An API endpoint for searching housing objects. 
        /// </summary>
        /// <param name="brokerId">The ID of the broker associated with the housing.</param>
        /// <param name="municipalityId">An optional municipality filter.</param>
        /// <returns>An embedded collection of <see cref="HousingDto"/>.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        [HttpGet]
        [ProducesResponseType<HousingDto>(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<HousingDto>>> Get([Required] int brokerId, int? municipalityId = null)
        {
            var housings = (await _housingRepository.GetAllHousingAsync(municipalityId, brokerId))
                .Select(x => _mapper.Map<HousingDto>(x))
                .ToList();

            housings.ForEach(x => x.Images = x.Images.Select(y => y = $"{_configuration.GetSection("FileStorage").GetSection("UploadFolderPath").Value}/{y}").ToList());

            return Ok(housings);
        }

        /// <summary>
        /// An API endpoint for fetching a housing object. 
        /// </summary>
        /// <param name="id">The ID of the housing to fetch.</param>
        /// <param name="brokerId">The ID of the broker associated with the housing.</param>
        /// <returns>An embedded collection of <see cref="HousingDto"/>.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        [HttpGet("{id:int}")]
        [ProducesResponseType<HousingDto>(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<HousingDto>>> GetById(int id, [Required] int brokerId)
        {
            var housing = await _housingRepository.GetHousingByIdAsync(id, brokerId);

            if (housing == null)
            {
                return NotFound();
            }

            var result = _mapper.Map<HousingDto>(housing);
            result.Images = result.Images.Select(x => x = $"{_configuration.GetSection("FileStorage").GetSection("UploadFolderPath").Value}/{x}").ToList();

            return Ok(result);
        }

        /// <summary>
        /// An API endpoint for creating housing objects. 
        /// </summary>
        /// <param name="brokerId">The ID of the broker associated with the new housing.</param>
        /// <param name="housingDto">The serialized DTO object.</param>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CreateHousingDto housingDto)
        {
            var newHousing = _mapper.Map<Housing>(housingDto);

            return Ok();

            await _housingRepository.AddAsync(newHousing);
            return Ok();
        }

        /// <summary>
        /// An API endpoint for updating housing objects. 
        /// </summary>
        /// <param name="id">The ID of the housing object to update.</param>
        /// <param name="brokerId">The ID of the broker associated with the new housing.</param>
        /// <param name="housingDto">The serialized DTO object.</param>
        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, [FromBody] UpdateHousingDto housingDto)
        {
            if (id != housingDto.HousingId)
            {
                return BadRequest();
            }

            var housing = _mapper.Map<Housing>(housingDto);

            return Ok();

            await _housingRepository.UpdateAsync(housing);
            return Ok();
        }

        #endregion
    }
}
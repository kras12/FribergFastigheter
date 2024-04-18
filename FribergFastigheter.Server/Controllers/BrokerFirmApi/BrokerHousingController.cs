using AutoMapper;
using FribergFastigheter.Data.Entities;
using FribergFastigheter.Server.Data.DTO;
using FribergFastigheter.Server.Data.Interfaces;
using FribergFastigheter.Server.Services;
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
    /// <!-- Co Authors: Marcus-->
    [Route("api/BrokerFirm/Housing")]
    [ApiController]
    public class BrokerHousingController : ControllerBase
    {
        #region Fields

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
        public BrokerHousingController(IHousingRepository housingRepository, IMapper mapper, IImageService imageService)
        {
            _housingRepository = housingRepository;
            _mapper = mapper;
            _imageService = imageService;
        }

        #endregion

        #region ApiEndPoints

        /// <summary>
        /// An API endpoint for deleting housing objects. 
        /// </summary>
        /// <param name="id">The ID of the housing object to delete.</param>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors:  -->
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
        /// <!-- Co Authors: Marcus -->
        [HttpGet]
        [ProducesResponseType<HousingDto>(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<HousingDto>>> Get([Required] int brokerId, int? municipalityId = null)
        {
            var housings = (await _housingRepository.GetAllHousingAsync(municipalityId, brokerId))
                .Select(x => _mapper.Map<HousingDto>(x))
                .ToList();

            _imageService.SetImageData(housings
                .SelectMany(x => x.Images).ToList());
            
            return Ok(housings);
        }

        /// <summary>
        /// An API endpoint for fetching a housing object. 
        /// </summary>
        /// <param name="id">The ID of the housing to fetch.</param>
        /// <param name="brokerId">The ID of the broker associated with the housing.</param>
        /// <returns>An embedded collection of <see cref="HousingDto"/>.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: Marcus -->
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
            _imageService.SetImageData(result.Images);

            return Ok(result);
        }

        /// <summary>
        /// An API endpoint for creating housing objects. 
        /// </summary>
        /// <param name="brokerId">The ID of the broker associated with the new housing.</param>
        /// <param name="housingDto">The serialized DTO object.</param>
        [HttpPost]
        public async Task<ActionResult> Post([Required] int brokerId, [FromBody] HousingDto housingDto)
        {
            var newHousing = _mapper.Map<Housing>(housingDto);

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
        public async Task<ActionResult> Put(int id, [Required] int brokerId, [FromBody] UpdateHousingDto housingDto)
        {
            if (id != housingDto.HousingId)
            {
                return BadRequest();
            }

            var housing = _mapper.Map<Housing>(housingDto);

            await _housingRepository.UpdateAsync(housing);
            return Ok();
        }

        #endregion
    }
}
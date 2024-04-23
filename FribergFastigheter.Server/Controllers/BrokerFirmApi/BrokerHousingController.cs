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

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FribergFastigheter.Server.Controllers.BrokerFirmApi
{
    /// <summary>
    /// An API controller for the broker firm housings API.
    /// </summary>
    /// <!-- Author: Jimmie -->
    /// <!-- Co Authors: Marcus-->
    [Route("api/BrokerFirm/Housing")]
    [ApiController]
    public class BrokerHousingController : ControllerBase
    {
		#region Fields

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
		public BrokerHousingController(IHousingRepository housingRepository, IMapper mapper, IImageService imageService, IBrokerFirmRepository brokerFirmRepository)
		{
			_housingRepository = housingRepository;
			_mapper = mapper;
			_imageService = imageService;
			_brokerFirmRepository = brokerFirmRepository;
		}

		#endregion

		#region ApiEndPoints

		/// <summary>
		/// An API endpoint for deleting housing objects. 
		/// </summary>
		/// <param name="id">The ID of the housing object to delete.</param>
		/// <param name="brokerFirmId">The ID of the broker firm associated with the housing.</param>
		/// <!-- Author: Jimmie -->
		/// <!-- Co Authors: -->
		[HttpDelete("{id:int}")]
		[ProducesResponseType<HousingDto>(StatusCodes.Status200OK)]
        [ProducesResponseType<ErrorMessageDto>(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Delete(int id, [Required] int brokerFirmId)
        {
            if (!await _housingRepository.Exists(id))
            {
                return NotFound(new ErrorMessageDto(HttpStatusCode.BadRequest, "The referenced housing object doesn't exists."));
            }
            else if (!await _housingRepository.IsOwnedByBrokerFirm(id, brokerFirmId))
			{
                return BadRequest(new ErrorMessageDto(HttpStatusCode.BadRequest, "The referenced housing object doesn't belong to the broker firm."));
            }            

            await _housingRepository.DeleteAsync(id);
            return Ok();
        }

		/// <summary>
		/// An API endpoint for searching housing objects. 
		/// </summary>
		/// <param name="brokerFirmId">The ID of the broker firm associated with the housing.</param>
		/// <param name="brokerId">The ID of the broker associated with the housing.</param>
		/// <param name="municipalityId">An optional municipality filter.</param>
		/// <returns>An embedded collection of <see cref="HousingDto"/>.</returns>
		/// <!-- Author: Jimmie -->
		/// <!-- Co Authors: -->
		[HttpGet]
        [ProducesResponseType<HousingDto>(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<HousingDto>>> Get([Required] int brokerFirmId, int? brokerId, int? municipalityId = null)
        {
            if (brokerId != null && !(await _brokerFirmRepository.HaveBroker(brokerFirmId, brokerId.Value)))
            {
				return BadRequest(new ErrorMessageDto(HttpStatusCode.BadRequest, "The referenced broker doesn't belong to the referenced broker firm."));
			}

			var housings = (await _housingRepository.GetAllHousingAsync(municipalityId, brokerId, brokerFirmId, limitImagesPerHousing: 3))
                .Select(x => _mapper.Map<HousingDto>(x))
                .ToList();

            _imageService.SetImageData(HttpContext, housings
                .SelectMany(x => x.Images).ToList());
            
            return Ok(housings);
        }

        /// <summary>
        /// An API endpoint for fetching a housing object. 
        /// </summary>
        /// <param name="id">The ID of the housing to fetch.</param>
        /// <param name="brokerFirmId">The ID of the broker firm associated with the housing.</param>
        /// <returns>An embedded collection of <see cref="HousingDto"/>.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: Marcus -->
        [HttpGet("{id:int}")]
        [ProducesResponseType<HousingDto>(StatusCodes.Status200OK)]
		[ProducesResponseType<ErrorMessageDto>(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<IEnumerable<HousingDto>>> GetById(int id, [Required] int brokerFirmId)
        {
            var housing = await _housingRepository.GetHousingByIdAsync(id);

            if (housing == null)
            {
                return NotFound();
            }
            else if (housing.BrokerFirm.BrokerFirmId != brokerFirmId)
            {
				return BadRequest(new ErrorMessageDto(HttpStatusCode.BadRequest, "The referenced housing object doesn't belong to the broker firm."));
			}

            var result = _mapper.Map<HousingDto>(housing);
            _imageService.SetImageData(HttpContext, result.Images);

            return Ok(result);
        }

		/// <summary>
		/// An API endpoint for creating housing objects. 
		/// </summary>
		/// param name="brokerFirmId">The ID of the broker firm associated with the housing.</param>
		/// <param name="createhousingDto">The serialized DTO object.</param>
		/// <!-- Author: Jimmie -->
		/// <!-- Co Authors: -->
		[HttpPost]
		[ProducesResponseType<HousingDto>(StatusCodes.Status200OK)]
		[ProducesResponseType<ErrorMessageDto>(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult> Post([Required] int brokerFirmId, [FromBody] CreateHousingDto newHousingDto)
        {
			if (brokerFirmId != newHousingDto.BrokerFirmId)
			{
				return BadRequest(new ErrorMessageDto(HttpStatusCode.BadRequest, "The referenced broker firm doesn't match the one in the posted housing object."));
			}

			var newHousingEntity = _mapper.Map<Housing>(newHousingDto);
            await _housingRepository.AddAsync(newHousingEntity);
            return Ok();
        }

		/// <summary>
		/// An API endpoint for updating housing objects. 
		/// </summary>
		/// <param name="id">The ID of the housing object to update.</param>
		/// <param name="brokerFirmId">The ID of the broker firm associated with the housing.</param>
		/// <param name="updateHousingDto">The serialized DTO object.</param>
		/// <!-- Author: Jimmie -->
		/// <!-- Co Authors: -->
		[HttpPut("{id:int}")]
		[ProducesResponseType<HousingDto>(StatusCodes.Status200OK)]
		[ProducesResponseType<ErrorMessageDto>(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult> Put([Required] int id, [Required] int brokerFirmId, [FromBody] UpdateHousingDto updateHousingDto)
        {
            if (id != updateHousingDto.HousingId || !await _housingRepository.IsOwnedByBrokerFirm(id, brokerFirmId))
            {
				return BadRequest(new ErrorMessageDto(HttpStatusCode.BadRequest, "The referenced housing object doesn't belong to the broker firm."));
			}
			else if (!await _housingRepository.HousingExists(id))
			{
				return BadRequest(new ErrorMessageDto(HttpStatusCode.BadRequest, "The referenced housing object doesn't exists."));
			}

    		var updatedHousingEntity = _mapper.Map<Housing>(updateHousingDto);
            await _housingRepository.UpdateAsync(updatedHousingEntity);
            return Ok();
        }

        #endregion
    }
}
using AutoMapper;
using FribergFastigheter.Server.Data.Entities;
using FribergFastigheter.Shared.Dto;
using FribergFastigheter.Server.Data.Interfaces;
using FribergFastigheter.Server.Data.Repositories;
using FribergFastigheter.Server.Services;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;
using FribergFastigheter.Shared.Dto.Statistics;
using FribergFastigheter.Server.Controllers.BrokerFirmApi;
using Microsoft.AspNetCore.Authorization;
using FribergFastigheter.Server.Data.Constants;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FribergFastigheter.Server.Controllers.BrokerApi
{
	/// <summary>
	/// An API controller for the broker firm image API.
	/// </summary>
	/// <!-- Author: Jimmie -->
	/// <!-- Co Authors: -->
	[Route("broker-firm-api")]
	[ApiController]
	public class BrokerFirmController : ControllerBase
	{
		#region Fields

		/// <summary>
		/// The injected housing repository.
		/// </summary>
		private readonly IBrokerFirmRepository _brokerFirmRepository;

		/// <summary>
		/// The injected Auto Mapper.
		/// </summary>
		private readonly IMapper _mapper;

        /// <summary>
        /// The injected imageService properties.
        /// </summary>
        private readonly IImageService _imageservice;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="brokerFirmRepository">The injected broker firm repository.</param>
        /// <param name="mapper">The injected Auto Mapper.</param>
        /// <param name="imageservice">The injected imageservice.</param>
        public BrokerFirmController(IBrokerFirmRepository brokerFirmRepository, IMapper mapper, IImageService imageservice)
		{
			_brokerFirmRepository = brokerFirmRepository;
			_mapper = mapper;
            _imageservice = imageservice;
		}

        #endregion

        #region ApiEndPoints

        /// <summary>
        /// An API endpoint for fetching a broker firm object.
        /// </summary>
        /// <param name="id">The ID of the broker firm to fetch.</param>
        /// <returns>An embedded collection of <see cref="BrokerFirmDto"/>.</returns>
        /// <!-- Author: Jimmie, Marcus -->
        /// <!-- Co Authors: -->
        [Authorize]
        [HttpGet("firm/{id:int}")]
		[ProducesResponseType<BrokerFirmDto>(StatusCodes.Status200OK)]
		[ProducesResponseType<ErrorMessageDto>(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<IEnumerable<BrokerFirmDto>>> GetBrokerFirmById(int id)
		{
			var brokerFirm = await _brokerFirmRepository.GetBrokerFirmByIdAsync(id);

			if (brokerFirm == null)
			{
				return NotFound(new ErrorMessageDto(System.Net.HttpStatusCode.NotFound, $"The broker firm with ID '{id}' was not found."));
			}

			if (int.Parse(User.FindFirst(ApplicationUserClaims.BrokerFirmId)!.Value) != id)
			{
                return BadRequest(new ErrorMessageDto(System.Net.HttpStatusCode.BadRequest, "Can't fetch data for another broker firm."));
            }

			var result = _mapper.Map<BrokerFirmDto>(brokerFirm);
            _imageservice.PrepareDto(HttpContext, BrokerFileController.ImageDownloadApiEndpoint, result);

			return Ok(result);
		}

        /// <summary>
        /// An API endpoint for fetching statistics for a broker firm. 
        /// </summary>
        /// <param name="id">The ID of the broker firm.</param>
        /// <returns>An embedded collection of <see cref="BrokerFirmStatisticsDto"/>.</returns>
		/// <!-- Author: Jimmie, Marcus -->
        /// <!-- Co Authors: -->
		[Authorize]
        [HttpGet("firm/{id:int}/statistics")]
        [ProducesResponseType<BrokerFirmStatisticsDto>(StatusCodes.Status200OK)]
        [ProducesResponseType<ErrorMessageDto>(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BrokerFirmStatisticsDto>> GetStatistics(int id)
		{
            if (int.Parse(User.FindFirst(ApplicationUserClaims.BrokerFirmId)!.Value) != id)
            {
                return BadRequest(new ErrorMessageDto(System.Net.HttpStatusCode.BadRequest, "Can't fetch data for another broker firm."));
            }

            if (!await _brokerFirmRepository.Exists(id))
            {
                return NotFound(new ErrorMessageDto(System.Net.HttpStatusCode.NotFound, $"The broker firm with ID '{id}' was not found."));
            }

			var result = await _brokerFirmRepository.GetStatistics(id);
            return Ok(result);
        }

		// TODO - Wait until identity is implemented before we decide whether to include or remove this feature. 
		/// <summary>
		/// An API endpoint for updating broker firms.
		/// </summary>
		/// <param name="id">The ID of the broker firm to update.</param>
		/// <param name="brokerFirmDto">The serialized DTO object.</param>
		/// <!-- Author: Jimmie -->
		/// <!-- Co Authors: -->
		//[HttpPut("firm/{id:int}")]
		//[ProducesResponseType<ErrorMessageDto>(StatusCodes.Status400BadRequest)]
		//public async Task<ActionResult> Update(int id, [FromBody] BrokerFirmDto brokerFirmDto)
		//{
		//	if (id != brokerFirmDto.BrokerFirmId)
		//	{
		//		return BadRequest(new ErrorMessageDto(HttpStatusCode.BadRequest, "The referenced broker firm doesn't match the supplied broker firm object."));
		//	}

		//	var brokerFirm = _mapper.Map<BrokerFirm>(brokerFirmDto);
		//	await _brokerFirmRepository.UpdateAsync(brokerFirm);
		//	return Ok();
		//}

		#endregion
	}
}

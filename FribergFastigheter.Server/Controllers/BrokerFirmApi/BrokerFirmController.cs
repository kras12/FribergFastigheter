using AutoMapper;
using FribergFastigheter.Server.Data.Entities;
using FribergFastigheter.Server.Data.Interfaces;
using FribergFastigheter.Server.Data.Repositories;
using FribergFastigheter.Server.Services;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;
using FribergFastigheter.Shared.Dto.Statistics;
using FribergFastigheter.Server.Controllers.BrokerFirmApi;
using Microsoft.AspNetCore.Authorization;
using FribergFastigheter.Shared.Constants;
using FribergFastigheter.Shared.Dto.BrokerFirm;
using FribergFastigheter.Shared.Dto.Error;

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
        [Authorize(policy: ApplicationPolicies.Broker)]
        [HttpGet("firm")]
		[ProducesResponseType<BrokerFirmDto>(StatusCodes.Status200OK)]
		[ProducesResponseType<ApiErrorResponseDto>(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<IEnumerable<BrokerFirmDto>>> GetBrokerFirmById()
		{
			var brokerFirmId = int.Parse(User.FindFirst(ApplicationUserClaims.BrokerFirmId)!.Value);
			var brokerFirm = await _brokerFirmRepository.GetBrokerFirmByIdAsync(brokerFirmId);

			if (brokerFirm == null)
			{
                return NotFound(new ApiErrorResponseDto(HttpStatusCode.NotFound, new ApiErrorDto(Shared.Enums.ApiErrorMessageTypes.ResourceNotFound, $"The broker firm with ID '{brokerFirmId}' was not found.")));
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
		[Authorize(policy: ApplicationPolicies.Broker)]
        [HttpGet("firm/statistics")]
        [ProducesResponseType<BrokerFirmStatisticsDto>(StatusCodes.Status200OK)]
        [ProducesResponseType<ApiErrorResponseDto>(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BrokerFirmStatisticsDto>> GetStatistics()
		{
            var brokerFirmId = int.Parse(User.FindFirst(ApplicationUserClaims.BrokerFirmId)!.Value);
            var result = await _brokerFirmRepository.GetStatistics(brokerFirmId);

            if (result == null)
            {
                return NotFound(new ApiErrorResponseDto(HttpStatusCode.NotFound, new ApiErrorDto(Shared.Enums.ApiErrorMessageTypes.ResourceNotFound, $"The broker firm with ID '{brokerFirmId}' was not found.")));
            }
			
            return Ok(result);
        }

		#endregion
	}
}

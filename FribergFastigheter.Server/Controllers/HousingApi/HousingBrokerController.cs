using AutoMapper;
using FribergFastigheter.Server.Data.Interfaces;
using FribergFastigheter.Server.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.ComponentModel.DataAnnotations;
using FribergFastigheter.Server.Controllers.BrokerFirmApi;
using FribergFastigheter.Shared.Dto.Broker;
using FribergFastigheter.Shared.Dto.Error;
using FribergFastigheter.Shared.Constants;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FribergFastigheter.Server.Controllers.HousingApi
{
    [Route("housing-api")]
	[ApiController]
	public class HousingBrokerController : ControllerBase
	{
		#region Fields

		/// <summary>
		/// The injected broker repository.
		/// </summary>
		private readonly IBrokerRepository _brokerRepository;

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
        /// <param name="brokerRepository">The injected broker repository.</param>
        /// <param name="mapper">The injected Auto Mapper.</param>
        /// <param name="imageService">The injected imageService.</param>
        public HousingBrokerController(IBrokerRepository brokerRepository, IMapper mapper, IImageService imageService)
		{
			_brokerRepository = brokerRepository;
			_mapper = mapper;
            _imageService = imageService;
        }

		#endregion

		#region ApiEndpoints

		/// <summary>
		/// An API endpoint for fetching a broker.
		/// </summary>
		/// <param name="id">The ID of the broker to fetch.</param>
		/// <returns>An embedded collection of <see cref="BrokerDto"/>.</returns>
		/// <!-- Author: Jimmie -->
		/// <!-- Co Authors: Marcus -->
		[HttpGet("broker/{id:int}")]
		[ProducesResponseType<BrokerDto>(StatusCodes.Status200OK)]
		[ProducesResponseType<ErrorMessageDto>(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<IEnumerable<BrokerDto>>> GetById(int id)
		{
			var broker = await _brokerRepository.GetBrokerByIdAsync(id);

			if (broker == null)
			{
				return NotFound(new ErrorMessageDto(System.Net.HttpStatusCode.NotFound, $"The broker with ID '{id}' was not found."));
			}

			var result = _mapper.Map<BrokerDto>(broker);
            _imageService.PrepareDto(HttpContext, HousingFileController.ImageDownloadApiEndpoint, result);

			return Ok(result);
		}

		/// <summary>
		/// An API endpoint for searching broker objects by brokerFirmId. 
		/// </summary>
		/// <returns>An embedded collection of <see cref="BrokerDto"/>.</returns>
		/// <!-- Author: Marcus, Jimmie -->
		/// <!-- Co Authors:  -->
		[HttpGet("brokers/{id:int}")]
		[ProducesResponseType<BrokerDto>(StatusCodes.Status200OK)]
		public async Task<ActionResult<IEnumerable<BrokerDto>>> GetBrokers(int id)
		{

			var brokers = (await _brokerRepository.GetBrokersAsync(id))
				.Select(x => _mapper.Map<BrokerDto>(x))
				.ToList();

			_imageService.PrepareDto(HttpContext, BrokerFileController.ImageDownloadApiEndpoint, brokers);
			return Ok(brokers);
		}

		#endregion
	}
}

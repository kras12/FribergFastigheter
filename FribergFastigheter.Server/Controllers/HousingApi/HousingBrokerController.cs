using AutoMapper;
using FribergFastigheter.Server.Data.DTO;
using FribergFastigheter.Server.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.ComponentModel.DataAnnotations;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FribergFastigheter.Server.Controllers.HousingApi
{
	[Route("api/Housing/Broker")]
	[ApiController]
	public class HousingBrokerController : ControllerBase
	{
		#region Fields

		/// <summary>
		/// The injected configuration properties.
		/// </summary>
		private readonly IConfiguration _configuration;

		/// <summary>
		/// The injected broker repository.
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
		public HousingBrokerController(IBrokerRepository brokerRepository, IMapper mapper, IConfiguration configuration)
		{
			_brokerRepository = brokerRepository;
			_mapper = mapper;
			_configuration = configuration;
		}

		#endregion

		#region ApiEndpoints

		/// <summary>
		/// An API endpoint for fetching a broker.
		/// </summary>
		/// <param name="id">The ID of the broker to fetch.</param>
		/// <returns>An embedded collection of <see cref="BrokerDto"/>.</returns>
		/// <!-- Author: Jimmie -->
		/// <!-- Co Authors: -->
		[HttpGet("{id:int}")]
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
			result.ProfileImage = $"{_configuration.GetSection("FileStorage").GetSection("UploadFolderPath").Value}/{result.ProfileImage}";

			return Ok(result);
		}

		#endregion
	}
}

using AutoMapper;
using FribergFastigheter.Server.Data.DTO;
using FribergFastigheter.Server.Data.Interfaces;
using FribergFastigheterApi.Data.DatabaseContexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FribergFastigheter.Server.Controllers.HousingApi
{
	[Route("api/Housing/BrokerFirm")]
	[ApiController]
	public class HousingBrokerFirmController : ControllerBase
	{
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
		/// <param name="brokerFirmRepository">The injected broker firm repository.</param>
		/// <param name="mapper">The injected Auto Mapper.</param>
		/// <param name="configuration">The injected configuration properties.</param>
		public HousingBrokerFirmController(IBrokerFirmRepository brokerFirmRepository, IMapper mapper, IConfiguration configuration)
		{
			_brokerFirmRepository = brokerFirmRepository;
			_mapper = mapper;
			_configuration = configuration;
		}

		#endregion

		#region ApiEndPoints

		/// <summary>
		/// An API endpoint for fetching a broker firm object.
		/// </summary>
		/// <param name="id">The ID of the broker firm to fetch.</param>
		/// <returns>An embedded collection of <see cref="BrokerFirmDto"/>.</returns>
		/// <!-- Author: Jimmie -->
		/// <!-- Co Authors: -->
		[HttpGet("{id:int}")]
		[ProducesResponseType<BrokerFirmDto>(StatusCodes.Status200OK)]
		public async Task<ActionResult<IEnumerable<BrokerFirmDto>>> GetById(int id)
		{
			var brokerFirm = await _brokerFirmRepository.GetBrokerFirmByIdAsync(id);

			if (brokerFirm == null)
			{
				return NotFound();
			}

			var result = _mapper.Map<BrokerFirmDto>(brokerFirm);
			result.Logotype = $"{_configuration.GetSection("FileStorage").GetSection("UploadFolderPath").Value}/{result.Logotype}";

			return Ok(result);
		}

		#endregion
	}
}

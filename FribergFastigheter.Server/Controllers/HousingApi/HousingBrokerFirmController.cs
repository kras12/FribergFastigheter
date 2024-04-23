using AutoMapper;
using FribergFastigheter.Shared.Dto;
using FribergFastigheter.Server.Data.Interfaces;
using FribergFastigheter.Server.Services;
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
        private readonly IImageService _imageService;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="brokerFirmRepository">The injected broker firm repository.</param>
        /// <param name="mapper">The injected Auto Mapper.</param>
        /// <param name="imageService">The injected imageService.</param>
        public HousingBrokerFirmController(IBrokerFirmRepository brokerFirmRepository, IMapper mapper, IImageService imageService)
		{
			_brokerFirmRepository = brokerFirmRepository;
			_mapper = mapper;
            _imageService = imageService;
        }

		#endregion

		#region ApiEndPoints

		/// <summary>
		/// An API endpoint for fetching a broker firm object.
		/// </summary>
		/// <param name="id">The ID of the broker firm to fetch.</param>
		/// <returns>An embedded collection of <see cref="BrokerFirmDto"/>.</returns>
		/// <!-- Author: Jimmie -->
		/// <!-- Co Authors: Marcus -->
		[HttpGet("{id:int}")]
		[ProducesResponseType<BrokerFirmDto>(StatusCodes.Status200OK)]
		[ProducesResponseType<ErrorMessageDto>(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<IEnumerable<BrokerFirmDto>>> GetById(int id)
		{
			var brokerFirm = await _brokerFirmRepository.GetBrokerFirmByIdAsync(id);

			if (brokerFirm == null)
			{
				return NotFound(new ErrorMessageDto(System.Net.HttpStatusCode.NotFound, $"The broker firm with ID '{id}' was not found."));
			}

			var result = _mapper.Map<BrokerFirmDto>(brokerFirm);
            if (result.Logotype != null)
            {
                _imageService.SetImageData(HttpContext, result.Logotype);
            }

			return Ok(result);
		}

		#endregion
	}
}

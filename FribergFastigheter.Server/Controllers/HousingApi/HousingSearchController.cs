using AutoMapper;
using FribergFastigheter.Data.Entities;
using FribergFastigheter.Shared.Dto;
using FribergFastigheter.Server.Data.Interfaces;
using FribergFastigheter.Server.Services;
using FribergFastigheterApi.Data.DatabaseContexts;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FribergFastigheter.Server.Controllers.HousingApi
{
    /// <summary>
    /// An API controller for the search housings API.
    /// </summary>
    /// <!-- Author: Marcus, Jimmie -->
    /// <!-- Co Authors: -->
    [Route("api/Housing/Search")]
    [ApiController]
    public class HousingSearchController : ControllerBase
    {
        #region Fields
        /// <summary>
        /// The injected housing repository.
        /// </summary>
        private readonly IHousingRepository _housingRepo;

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

        public HousingSearchController(IHousingRepository housingRepo, IMapper mapper, IImageService imageService)
        {
            _housingRepo = housingRepo;
            _mapper = mapper;
            _imageService = imageService;
        }

		#endregion

		#region ApiEndPoints

		/// <summary>
		/// An API endpoint for searching housing objects. 
		/// </summary>
		/// <param name="municipalityId">An optional municipality filter.</param>
		/// <param name="limitHousings">An optional max limit for number of retrieved housings.</param>
		/// <param name="limitImageCountPerHousing">An optional max limit for number of retrieved images per housing.</param>
		/// <returns>An embedded collection of <see cref="HousingDto"/>.</returns>
		/// <!-- Author: Marcus -->
		/// <!-- Co Authors: Jimmie -->
		[HttpGet]
        [ProducesResponseType<HousingDto>(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<HousingDto>>> GetHousings(int? municipalityId = null, int? limitHousings = null, int? limitImageCountPerHousing = null)
        {
            var housings = (await _housingRepo.GetAllHousingAsync(municipalityId: municipalityId, limitHousings: limitHousings, limitImagesPerHousing: limitImageCountPerHousing))
                .Select(x => _mapper.Map<HousingDto>(x))
                .ToList();

            _imageService.SetImageData(housings
                .SelectMany(x => x.Images).ToList());

            return Ok(housings);
        }

        /// <summary>
        /// An API endpoint for retrieving a housing object. 
        /// </summary>
        /// <param name="id">The ID of the housing object to fetch.</param>
        /// <returns>An embedded <see cref="HousingDto"/> object.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: Marcus -->
        [HttpGet("{id:int}")]
        [ProducesResponseType<HousingDto>(StatusCodes.Status200OK)]
		[ProducesResponseType<ErrorMessageDto>(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<HousingDto>> GetHousing(int id)
        {
            var housing = await _housingRepo.GetHousingByIdAsync(id);

            if (housing == null)
            {
				return NotFound(new ErrorMessageDto(System.Net.HttpStatusCode.NotFound, $"The housing object with ID '{id}' was not found."));
			}

            var result = _mapper.Map<HousingDto>(housing);
            _imageService.SetImageData(result.Images);

            return Ok(result);
        }

        #endregion
    }
}

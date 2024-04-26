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
        /// <param name="categoryId">An optional housing category filter.</param>
        /// <param name="limitHousings">An optional max limit for number of retrieved housings.</param>
        /// <param name="limitImageCountPerHousing">An optional max limit for number of retrieved images per housing.</param>
        /// <param name="minPrice">An optional min price filter.</param>
        /// <param name="maxPrice">An optional max price filter.</param>
        /// <param name="minLivingArea">An optional min living area filter.</param>
        /// <param name="maxLivingArea">An optional max living area filter.</param>
        /// <returns>An embedded collection of <see cref="HousingDto"/>.</returns>
        /// <!-- Author: Marcus -->
        /// <!-- Co Authors: Jimmie -->
        [HttpGet]
        [ProducesResponseType<HousingDto>(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<HousingDto>>> GetHousings(int? municipalityId = null, int? housingCategoryId = null, 
            int? limitHousings = null, int? limitImageCountPerHousing = null,
            decimal? minPrice = null, decimal? maxPrice = null, double? minLivingArea = null, double? maxLivingArea = null)
        {
            var housings = (await _housingRepo.GetAllHousingsAsync(municipalityId: municipalityId, housingCategoryId: housingCategoryId, 
                    limitHousings: limitHousings, limitImagesPerHousing: limitImageCountPerHousing, 
                    minPrice: minPrice, maxPrice: maxPrice, minLivingArea: minLivingArea, maxLivingArea: maxLivingArea))
                .Select(x => _mapper.Map<HousingDto>(x))
                .ToList();

            _imageService.PrepareDto(HttpContext, housings);

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
            _imageService.PrepareDto(HttpContext, result);

            return Ok(result);
        }

        #endregion
    }
}

using AutoMapper;
using FribergFastigheter.Server.Data.Entities;
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
    [Route("housing-api")]
    [ApiController]
    public class HousingController : ControllerBase
    {
        #region Fields
        /// <summary>
        /// The injected housing repository.
        /// </summary>
        private readonly IHousingRepository _housingRepository;

        /// <summary>
        /// The injected imageService properties.
        /// </summary>
        private readonly IImageService _imageService;

        /// <summary>
        /// The injected Auto Mapper.
        /// </summary>
        private readonly IMapper _mapper;
        #endregion

        #region Constructors

        public HousingController(IHousingRepository housingRepo, IMapper mapper, IImageService imageService)
        {
            _housingRepository = housingRepo;
            _mapper = mapper;
            _imageService = imageService;
        }

        #endregion

        #region ApiEndPoints		

        /// <summary>
        /// An API endpoint for fetching all housing categories.
        /// </summary>
        /// <returns>An embedded collection of <see cref="HousingCategoryDto"/>.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        [HttpGet("housing/categories")]
        [ProducesResponseType<HousingDto>(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<HousingCategoryDto>>> GetHousingCategories()
        {
            return Ok(_mapper.Map<List<HousingCategoryDto>>(await _housingRepository.GetHousingCategories()));
        }

        /// <summary>
        /// An API endpoint for retrieving a housing object. 
        /// </summary>
        /// <param name="id">The ID of the housing object to fetch.</param>
        /// <returns>An embedded <see cref="HousingDto"/> object.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: Marcus -->
		[HttpGet("housing/{id:int}")]
        [ProducesResponseType<HousingDto>(StatusCodes.Status200OK)]
        [ProducesResponseType<ErrorMessageDto>(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<HousingDto>> GetHousingById(int id)
        {
            var housing = await _housingRepository.GetHousingByIdAsync(id);

            if (housing == null)
            {
                return NotFound(new ErrorMessageDto(System.Net.HttpStatusCode.NotFound, $"The housing object with ID '{id}' was not found."));
            }

            var result = _mapper.Map<HousingDto>(housing);
            _imageService.PrepareDto(HttpContext, HousingFileController.ImageDownloadApiEndpoint, result);

            return Ok(result);
        }

        /// <summary>
        /// An API endpoint for searching housing objects. 
        /// </summary>
        /// <param name="brokerId">An optional broker filter.</param>
        /// <param name="municipalityId">An optional municipality filter.</param>
        /// <param name="housingCategoryId">An optional housing category filter.</param>
        /// <param name="limitHousings">An optional max limit for number of retrieved housings.</param>
        /// <param name="limitImagesPerHousing">An optional max limit for number of retrieved images per housing.</param>
        /// <param name="minPrice">An optional min price filter.</param>
        /// <param name="maxPrice">An optional max price filter.</param>
        /// <param name="minLivingArea">An optional min living area filter.</param>
        /// <param name="maxLivingArea">An optional max living area filter.</param>
        /// <param name="offsetRows">An optional number of rows to skip.</param>
        /// <returns>A <see cref="HousingSearchResultDto"/> object containing the results.</returns>
        /// <!-- Author: Marcus -->
        /// <!-- Co Authors: Jimmie -->
        [HttpGet("housings")]
        [ProducesResponseType<HousingSearchResultDto>(StatusCodes.Status200OK)]
        public async Task<ActionResult<HousingSearchResultDto>> SearchHousings(int? brokerId = null, int? municipalityId = null, int? housingCategoryId = null,
            int? limitHousings = null, int? limitImagesPerHousing = null,
            decimal? minPrice = null, decimal? maxPrice = null, double? minLivingArea = null, double? maxLivingArea = null, int? offsetRows = null)
        {
            var result = new HousingSearchResultDto();

            result.Housings = (await _housingRepository.GetHousingsAsync(brokerId: brokerId, municipalityId: municipalityId, housingCategoryId: housingCategoryId,
                    limitHousings: limitHousings, limitImagesPerHousing: limitImagesPerHousing,
                    minPrice: minPrice, maxPrice: maxPrice, minLivingArea: minLivingArea, maxLivingArea: maxLivingArea, offsetRows: offsetRows))
                .Select(x => _mapper.Map<HousingDto>(x))
                .ToList();

            _imageService.PrepareDto(HttpContext, HousingFileController.ImageDownloadApiEndpoint, result.Housings);

            if (limitHousings != null && result.Housings.Count > 0)
            {
                result.Pagination = new PaginationDto();

                result.Pagination.TotalResults = await _housingRepository.GetHousingsCountAsync(municipalityId: municipalityId, housingCategoryId: housingCategoryId,
                    minPrice: minPrice, maxPrice: maxPrice,
                    minLivingArea: minLivingArea, maxLivingArea: maxLivingArea);

                result.Pagination.PageSize = limitHousings.Value;
                result.Pagination.CurrentPage = offsetRows != null ? offsetRows.Value / limitHousings.Value + 1 : 1;
            }

            return Ok(result);
        }

        /// <summary>
        /// An API endpoint for fetching all municipalities.
        /// </summary>
        /// <returns>An embedded collection of <see cref="MunicipalityDto"/>.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        [HttpGet("municipalities")]
        [ProducesResponseType<MunicipalityDto>(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<MunicipalityDto>>> GetMunicipalities()
        {
            return Ok(_mapper.Map<List<MunicipalityDto>>(await _housingRepository.GetMunicipalities()));
        }

        #endregion
    }
}

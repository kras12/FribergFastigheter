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
    /// An API controller for the housing category API.
    /// </summary>
    /// <!-- Author: Jimmie -->
    /// <!-- Co Authors: -->
    [Route("api/Housing/Category")]
    [ApiController]
    public class HousingCategoryController : ControllerBase
    {
        #region Fields
        /// <summary>
        /// The injected housing repository.
        /// </summary>
        private readonly IHousingRepository _housingRepository;

        /// <summary>
        /// The injected Auto Mapper.
        /// </summary>
        private readonly IMapper _mapper;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="housingRepo">The injected housing repository.</param>
        /// <param name="mapper">The injected Auto Mapper.</param>
        /// 
        public HousingCategoryController(IHousingRepository housingRepo, IMapper mapper)
        {
            _housingRepository = housingRepo;
            _mapper = mapper;
        }

        #endregion

        #region ApiEndPoints

        /// <summary>
        /// An API endpoint for fetching all housing categories.
        /// </summary>
        /// <returns>An embedded collection of <see cref="HousingCategoryDto"/>.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        [HttpGet]
        [ProducesResponseType<HousingDto>(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<HousingCategoryDto>>> GetCategories()
        {
            return Ok(_mapper.Map<List<HousingCategoryDto>>(await _housingRepository.GetHousingCategories()));
        }

        #endregion
    }
}

using AutoMapper;
using FribergFastigheter.Data.Entities;
using FribergFastigheter.Shared.Dto;
using FribergFastigheter.Server.Data.Interfaces;
using FribergFastigheter.Server.Services;
using FribergFastigheterApi.Data.DatabaseContexts;
using Microsoft.AspNetCore.Mvc;

namespace FribergFastigheter.Server.Controllers.HousingApi
{
    /// <summary>
    /// An API controller for the housing municipality API.
    /// </summary>
    /// <!-- Author: Jimmie -->
    /// <!-- Co Authors: -->
    [Route("api/Housing/Municipality")]
    [ApiController]
    public class HousingMunicipalityController : ControllerBase
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
        public HousingMunicipalityController(IHousingRepository housingRepo, IMapper mapper)
        {
            _housingRepository = housingRepo;
            _mapper = mapper;
        }

        #endregion

        #region ApiEndPoints

        /// <summary>
        /// An API endpoint for fetching all municipalities.
        /// </summary>
        /// <returns>An embedded collection of <see cref="MunicipalityDto"/>.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        [HttpGet]
        [ProducesResponseType<MunicipalityDto>(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<MunicipalityDto>>> GetCategories()
        {
            return Ok(_mapper.Map<List<MunicipalityDto>>(await _housingRepository.GetMunicipalities()));
        }

        #endregion
    }
}

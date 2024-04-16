using AutoMapper;
using FribergFastigheter.Data.Entities;
using FribergFastigheter.Server.Data.DTO;
using FribergFastigheter.Server.Data.Interfaces;
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

        private readonly IHousingRepository _housingRepo;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        #endregion

        #region Constructors

        public HousingSearchController(IHousingRepository housingRepo, IMapper mapper, IConfiguration configuration)
        {
            _housingRepo = housingRepo;
            _mapper = mapper;
            _configuration = configuration;
        }

        #endregion

        #region ApiEndPoints

        /// <summary>
        /// An API endpoint for searching housing objects. 
        /// </summary>
        /// <param name="municipalityId">An optional municipality filter.</param>
        /// <returns>An embedded collection of <see cref="HousingDto"/>.</returns>
        /// <!-- Author: Marcus -->
        /// <!-- Co Authors: Jimmie -->
        [HttpGet]
        [ProducesResponseType<HousingDto>(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<HousingDto>>> GetHousings(int? municipalityId = null)
        {
            var housings = (await _housingRepo.GetAllHousingAsync(municipalityId))
                .Select(x => _mapper.Map<HousingDto>(x))
                .ToList();

            housings.ForEach(x => x.Images = x.Images.Select(y => y = $"{_configuration.GetSection("FileStorage").GetSection("UploadFolderPath").Value}/{y}").ToList());

            return Ok(housings);
        }

        /// <summary>
        /// An API endpoint for retrieving a housing object. 
        /// </summary>
        /// <param name="id">The ID of the housing object to fetch.</param>
        /// <returns>An embedded <see cref="HousingDto"/> object.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors:  -->
        [HttpGet("{id:int}")]
        [ProducesResponseType<HousingDto>(StatusCodes.Status200OK)]
        public async Task<ActionResult<HousingDto>> GetHousing(int id)
        {
            var housing = await _housingRepo.GetHousingByIdAsync(id);

            if (housing == null)
            {
                return NotFound();
            }

            var result = _mapper.Map<HousingDto>(housing);
            result.Images = result.Images.Select(x => x = $"{_configuration.GetSection("FileStorage").GetSection("UploadFolderPath").Value}/{x}").ToList();

            return Ok(result);
        }

        #endregion
    }
}

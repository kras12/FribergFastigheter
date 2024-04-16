using AutoMapper;
using FribergFastigheter.Data.Entities;
using FribergFastigheter.Server.Data.DTO;
using FribergFastigheter.Server.Data.Interfaces;
using FribergFastigheterApi.Data.DatabaseContexts;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FribergFastigheter.Server.Controllers
{
	/// <summary>
	/// An API controller for the search housings API.
	/// </summary>
	/// <!-- Author: Marcus, Jimmie -->
	/// <!-- Co Authors: -->
	[Route("api/Search/Housing")]
	[ApiController]
	public class SearchHousingController : ControllerBase
	{

		#region Fields

		private readonly IHousingRepository _housingRepo;
		private readonly IMapper _mapper;
		private readonly IConfiguration _configuration;

		#endregion

		#region Constructors

		public SearchHousingController(IHousingRepository housingRepo, IMapper mapper, IConfiguration configuration)
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
		// GET: api/<HousingController>
		[HttpGet]
		[ProducesResponseType<Housing>(StatusCodes.Status200OK)]
		public async Task<ActionResult<List<HousingDto>>> GetHousings(int? municipalityId = null)
		{
			var housings = (await _housingRepo.GetAllHousingAsync(municipalityId))
				.Select(x => _mapper.Map<HousingDto>(x))
				.ToList();

			housings.ForEach(x => x.Images = x.Images.Select(y => y = $"{_configuration.GetSection("FileStorage").GetSection("UploadFolderPath").Value}/{y}").ToList());

			return Ok(housings);
		}

		#endregion
	}
}

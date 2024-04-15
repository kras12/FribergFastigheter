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
	/// An API controller for searching housing objects.
	/// </summary>
	[Route("api/[controller]")]
	[ApiController]
	/// <!-- Author: Marcus -->
	/// <!-- Co Authors: -->
	public class SearchHousingController : ControllerBase
	{

		#region Fields

		private readonly IHousingRepository _housingRepo;
		private readonly IMapper _mapper;

		#endregion

		#region Constructors

		public SearchHousingController(IHousingRepository housingRepo, IMapper mapper)
		{
			_housingRepo = housingRepo;
			_mapper = mapper;
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
			return Ok((await _housingRepo.GetAllHousingAsync(municipalityId))
				.Select(x => _mapper.Map<HousingDto>(x)).ToList());
		}

		#endregion
	}
}

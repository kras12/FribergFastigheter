﻿using AutoMapper;
using FribergFastigheter.Data.Entities;
using FribergFastigheter.Server.Data.DTO;
using FribergFastigheter.Server.Data.Interfaces;
using FribergFastigheter.Server.Data.Repositories;
using FribergFastigheterApi.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FribergFastigheter.Server.Controllers.BrokerApi
{
	/// <summary>
	/// An API controller for the broker firm API.
	/// </summary>
	/// <!-- Author: Jimmie -->
	/// <!-- Co Authors: -->
	[Route("api/BrokerFirm")]
	[ApiController]
	public class BrokerFirmController : ControllerBase
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
		public BrokerFirmController(IBrokerFirmRepository brokerFirmRepository, IMapper mapper, IConfiguration configuration)
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
		[ProducesResponseType<ErrorMessageDto>(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<IEnumerable<BrokerFirmDto>>> GetById(int id)
		{
			var brokerFirm = await _brokerFirmRepository.GetBrokerFirmByIdAsync(id);

			if (brokerFirm == null)
			{
				return NotFound(new ErrorMessageDto(System.Net.HttpStatusCode.NotFound, $"The broker firm with ID '{id}' was not found."));
			}

			var result = _mapper.Map<BrokerFirmDto>(brokerFirm);
			result.Logotype = $"{_configuration.GetSection("FileStorage").GetSection("UploadFolderPath").Value}/{result.Logotype}";

			return Ok(result);
		}

		/// <summary>
		/// An API endpoint for updating broker firms.
		/// </summary>
		/// <param name="id">The ID of the broker firm to update.</param>
		/// <param name="brokerFirmDto">The serialized DTO object.</param>
		/// <!-- Author: Jimmie -->
		/// <!-- Co Authors: -->
		[HttpPut("{id:int}")]
		[ProducesResponseType<ErrorMessageDto>(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult> Put(int id, [FromBody] BrokerFirmDto brokerFirmDto )
		{
			if (id != brokerFirmDto.BrokerFirmId)
			{
				return BadRequest(new ErrorMessageDto(HttpStatusCode.BadRequest, "The referenced broker firm doesn't match the supplied broker firm object."));
			}

			var brokerFirm = _mapper.Map<BrokerFirm>(brokerFirmDto);
			await _brokerFirmRepository.UpdateAsync(brokerFirm);
			return Ok();
		}

		#endregion
	}
}
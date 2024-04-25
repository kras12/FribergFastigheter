using Microsoft.AspNetCore.Components;
using System.Diagnostics;
using System.Net.Http.Json;
using FribergFastigheter.Shared.Dto;
using FribergFastigheter.Client.Services.HousingApi;
using FribergFastigheter.Client.Models;
using AutoMapper;

namespace FribergFastigheter.Client.Pages
{
    /// <summary>
    /// The website home page component.
    /// </summary>
    public partial class Home : ComponentBase
    {
		#region Properties

#pragma warning disable CS8618
		/// <summary>
		/// The inject Api service for Friberg Fastigheter.
		/// </summary>
		[Inject] 
		private IHousingApiService ApiService { get; set; }
#pragma warning restore CS8618

#pragma warning disable CS8618
        /// <summary>
        /// The injected auto mapper.
        /// </summary>
        [Inject]
		private IMapper AutoMapper { get; set; }
#pragma warning restore CS8618

        /// <summary>
        /// A test property to test the API.
        /// </summary>
        public List<HousingViewModel> Housings { get; set; } = new();

		#endregion

		#region Methods

		/// <summary>
		/// Method invoked when the component is ready to start, having received its
		/// initial parameters from its parent in the render tree.
		/// </summary>
		/// <returns>A <see cref="Task"/> representing any asynchronous operation.</returns>
		protected async override Task OnInitializedAsync()
		{
			await base.OnInitializedAsync();
            var result = await ApiService.SearchHousings(limitHousings: 10, limitImageCountPerHousing: 3);

			if (result != null)
			{
				Housings = result.Select(x => AutoMapper.Map<HousingViewModel>(x)).ToList();
			}
		}

        #endregion
    }	
}

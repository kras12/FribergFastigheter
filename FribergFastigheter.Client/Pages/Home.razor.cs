using Microsoft.AspNetCore.Components;
using System.Diagnostics;
using System.Net.Http.Json;
using FribergFastigheter.Shared.Dto;
using FribergFastigheter.Client.Services;

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
		private IFribergFastigheterApiService ApiService { get; set; }
#pragma warning restore CS8618 

		/// <summary>
		/// A test property to test the API.
		/// </summary>
		public int HouseFetchCount { get; set; }

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

			var housings = await ApiService.SearchHousings(limitHousings: 25, limitImageCountPerHousing: 3);

			if (housings != null)
			{
				HouseFetchCount = housings.Count;
			}
		}

		#endregion
	}
}

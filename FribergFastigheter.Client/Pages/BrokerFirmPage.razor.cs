using AutoMapper;
using FribergFastigheter.Client.Models.BrokerFirm;
using FribergFastigheter.Client.Services.FribergFastigheterApi;
using Microsoft.AspNetCore.Components;

namespace FribergFastigheter.Client.Pages
{
    /// <summary>
    /// The broker firm details page. 
    /// </summary>
    /// <!-- Author: Jimmie -->
    /// <!-- Co Authors: -->
    public partial class BrokerFirmPage : ComponentBase
	{
		#region InjectedServiceProperties
#pragma warning disable CS8618

		/// <summary>
		/// The injected AutoMapper service.
		/// </summary>
		[Inject]
		public IMapper AutoMapper { get; set; }

		/// <summary>
		/// The injected housing API service.
		/// </summary>
		[Inject]
		public IHousingApiService HousingApiService { get; set; }

#pragma warning restore CS8618
		#endregion

		#region Properties

		/// <summary>
		/// The broker firm.
		/// </summary>
		public BrokerFirmViewModel? BrokerFirm { get; set; } = null;

		/// <summary>
		/// The ID of the broker firm.
		/// </summary>
		[Parameter]
		public int BrokerFirmId { get; set; }

		#endregion

		#region Methods

		/// <summary>
		/// Method invoked when the component is ready to start, having received its initial
		/// parameters from its parent in the render tree. Override this method if you will
		/// perform an asynchronous operation and want the component to refresh when that
		/// operation is completed. 
		/// </summary>
		/// <returns>A <see cref="Task"/> representing any asynchronous operation.</returns>
		protected async override Task OnInitializedAsync()
		{
			await base.OnInitializedAsync();
            BrokerFirm = AutoMapper.Map<BrokerFirmViewModel>(await HousingApiService.GetBrokerFirmById(BrokerFirmId));		
		}

		#endregion
	}
}

using AutoMapper;
using FribergFastigheter.Client.Models.Broker;
using FribergFastigheter.Client.Models.Housing;
using FribergFastigheter.Client.Services.FribergFastigheterApi;
using Microsoft.AspNetCore.Components;

namespace FribergFastigheter.Client.Pages
{
    /// <summary>
    /// The housing broker page. 
    /// This page displays the information about the housing object's broker.
    /// </summary>
    /// <!-- Author: Jimmie -->
    /// <!-- Co Authors: -->
    public partial class BrokerDetailsPage : ComponentBase
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
		/// The broker associated with the housing object.
		/// </summary>
		public BrokerViewModel? Broker { get; set; } = null;

		/// <summary>
		/// The ID of the broker.
		/// </summary>
		[Parameter]
		public int BrokerId { get; set; }

		/// <summary>
		/// The housing objects that is handled by the broker. 
		/// </summary>
		public List<HousingViewModel>? BrokerHousings { get; set; } = null;

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

			var response = await HousingApiService.GetBrokerById(BrokerId);

			if (response.Success)
			{
				Broker = AutoMapper.Map<BrokerViewModel>(response.Value!);
				var innerResponse = await HousingApiService.GetHousingsByBroker(Broker.BrokerId, 3);

				if (innerResponse.Success)
				{
					BrokerHousings = AutoMapper.Map<List<HousingViewModel>>(innerResponse.Value!);
					// TODO - Find a better way to retrieve the URLS
					BrokerHousings.ForEach(x => x.Url = $"Housing/{x.HousingId}");
					BrokerHousings.Select(x => x.Broker).ToList().ForEach(x => x.Url = $"Broker/{x.BrokerId}");
					BrokerHousings.Select(x => x.Broker.BrokerFirm).ToList().ForEach(x => x.Url = $"BrokerFirm/{x.BrokerFirmId}");
				}
				else
				{
					// TODO - Handle
				}
			}
			else
			{
				// TODO - Handle
			}				
		}

		#endregion
	}
}

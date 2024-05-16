using AutoMapper;
using FribergFastigheter.Client.Models.Broker;
using FribergFastigheter.Client.Models.BrokerFirm;
using FribergFastigheter.Client.Models.Housing;
using FribergFastigheter.Client.Services.FribergFastigheterApi;
using FribergFastigheter.Shared.Dto.BrokerFirm;
using Microsoft.AspNetCore.Components;

namespace FribergFastigheter.Client.Pages
{
    /// <summary>
    /// The broker firm details page. 
    /// </summary>
    /// <!-- Author: Marcus -->
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
		/// List of broker working for the brokerfirm.
		/// </summary>
		public List<BrokerViewModel> Brokers { get; set; }

		/// <summary>
		/// List of housings the brokerfirm have.
		/// </summary>
		public List<HousingViewModel> Housings { get; set; }

		/// <summary>
		/// The broker firm.
		/// </summary>
		public BrokerFirmViewModel? BrokerFirm { get; set; } = null;

		/// <summary>
		/// The ID of the broker firm.
		/// </summary>
		[Parameter]
		public int BrokerFirmId { get; set; }

		public bool IsBrokerListorHousingListActive { get; set; } = true;

		#endregion

		#region Constructors

		public BrokerFirmPage()
		{

		}

		#endregion

		#region Methods

		/// <summary>
		/// Method invoked when the component is ready to start, having received its initial
		/// parameters from its parent in the render tree. Override this method if you will
		/// perform an asynchronous operation and want the component to refresh when that
		/// operation is completed. 
		/// </summary>
		/// <returns>A <see cref="Task"/> representing any asynchronous operation.</returns>
		protected override async Task OnInitializedAsync()
		{
			var response = await HousingApiService.GetBrokerFirmById(BrokerFirmId);

			if (response.Success)
			{
				BrokerFirmDto brokerFirm = response.Value!;
				BrokerFirmViewModel brokerFirmResult = AutoMapper.Map<BrokerFirmViewModel>(brokerFirm);
				BrokerFirm = brokerFirmResult;

				Brokers = BrokerFirm.Brokers;

				var housingsResponse = await HousingApiService.GetHousings(BrokerFirmId, 3);
				if (housingsResponse.Success)
				{
					Housings = AutoMapper.Map<List<HousingViewModel>>(housingsResponse.Value);
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

		public void OpenBrokerList()
		{
			IsBrokerListorHousingListActive = true;
		}

		public void OpenHousingList()
		{
			IsBrokerListorHousingListActive = false;
		}

		#endregion
	}
}

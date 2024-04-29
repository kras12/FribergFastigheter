using AutoMapper;
using FribergFastigheter.Client.Models;
using FribergFastigheter.Client.Services.FribergFastigheterApi;
using Microsoft.AspNetCore.Components;

namespace FribergFastigheter.Client.Pages
{
    /// <summary>
    /// The housing details page.
    /// </summary>
    public partial class HousingDetailsPage : ComponentBase
    {
        #region Fields

        /// <summary>
        /// The housing object.
        /// </summary>
        private HousingViewModel? _housingViewModel = null;

        #endregion

        #region InjectedProperties
#pragma warning disable CS8618

        /// <summary>
        /// The injected auto mapper service.
        /// </summary>
        [Inject]
        private IMapper AutoMapper { get; set; }

        /// <summary>
        /// The injected housing API service.
        /// </summary>
        [Inject]
        private IHousingApiService HousingApiService { get; set; }

#pragma warning restore CS8618
        #endregion

        #region OtherProperties

        [Parameter]
		public int Id { get; set; }

        #endregion

        #region Methods


        /// <summary>
        /// Method invoked when the component is ready to start, having received its
        /// initial parameters from its parent in the render tree.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing any asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            var result = await HousingApiService.GetHousingById(Id);

            if (result != null)
            {
                _housingViewModel = AutoMapper.Map<HousingViewModel>(result);

				// TODO - Find a better way to retrieve the URLS
				_housingViewModel.Broker.Url = $"Broker/{_housingViewModel.Broker.BrokerId}";
				_housingViewModel.Broker.BrokerFirm.Url = $"BrokerFirm/{_housingViewModel.Broker.BrokerFirm.BrokerFirmId}";
			}
        }

        #endregion
    }
}

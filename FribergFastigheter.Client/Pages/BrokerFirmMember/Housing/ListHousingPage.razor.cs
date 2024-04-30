using AutoMapper;
using FribergFastigheter.Client.Components;
using FribergFastigheter.Client.Models;
using FribergFastigheter.Client.Services.FribergFastigheterApi;
using FribergFastigheter.Shared.Dto;
using Microsoft.AspNetCore.Components;

namespace FribergFastigheter.Client.Pages.BrokerFirmMember.Housing
{
    /// <summary>
    /// The list housing page in the broker firm back office.
    /// </summary>
    public partial class ListHousingPage : ComponentBase
    {
        #region Fields       

        /// <summary>
        /// Temporary ID variable for which broker firm to use performing actions.
        /// </summary>
        private int _brokerFirmId = 1;

        /// <summary>
        /// The ID of the broker to assign when creating a new housing object.
        /// </summary>
        private int _createHousingBrokerId = 1;

        /// <summary>
        /// A reference to the housing list component. 
        /// </summary>
        private HousingList _housingListComponent;

        /// <summary>
        /// A collection of housing objects to show in the listing.
        /// </summary>
        private List<HousingViewModel> _housings = new();

        #endregion

        #region InjectServiceProperties
#pragma warning disable CS8618

        /// <summary>
        /// The injected Auto Mapper service.
        /// </summary>
        [Inject]
        private IMapper AutoMapper { get; set; }

        /// <summary>
        /// The injected broker firm API service. 
        /// </summary>
        [Inject]
        private IBrokerFirmApiService BrokerFirmApiService { get; set; }

#pragma warning restore CS8618
        #endregion

        #region Properties

        #endregion

        #region Methods

        /// <summary>
        /// Method invoked when the component is ready to start, having received its initial 
        /// parameters from its parent in the render tree. Override this method if you will 
        /// perform an asynchronous operation and want the component to refresh when that 
        /// operation is completed.
        /// </summary>
        /// <returns>A System.Threading.Tasks.Task representing any asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            _housings = AutoMapper.Map<List<HousingViewModel>>(await BrokerFirmApiService.GetHousings(_brokerFirmId, limitImagesPerHousing: 3));
        }

        /// <summary>
        /// Event handler for the on house created event. 
        /// </summary>
        /// <param name="createdHousing">The new housing object.</param>
        private void OnHousingCreated(HousingViewModel createdHousing)
        {
            _housings.Add(createdHousing);
            _housingListComponent.ScrollToElement(createdHousing);
        }

		/// <summary>
		/// Event handler for the on house edited event. 
		/// </summary>
		/// <param name="editedHousing">The edtied housing object.</param>
		private void OnHousingEdited(HousingViewModel editedHousing)
		{
			_housingListComponent.ScrollToElement(editedHousing);
		}

		#endregion
	}
}

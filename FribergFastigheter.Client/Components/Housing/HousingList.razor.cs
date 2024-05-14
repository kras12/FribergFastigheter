using FribergFastigheter.Client.Models.Broker;
using FribergFastigheter.Client.Models.Housing;
using FribergFastigheter.Client.Services.FribergFastigheterApi;
using FribergFastigheter.Shared.Constants;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;

namespace FribergFastigheter.Client.Components.Housing
{
    /// <summary>
    /// A component that shows a housing list with support for filtering by broker.
    /// </summary>
    /// <!-- Author: Jimmie -->
    /// <!-- Co Authors: -->
    public partial class HousingList : ComponentBase
    {
        #region Enums

        /// <summary>
        /// The types of filtering that can be performed. 
        /// </summary>
        public enum FilterTypes
        {
            All,
            Brokers
        }

        #endregion

        #region Fields

		/// <summary>
		/// The current filter applied to the list.
		/// </summary>
		private FilterTypes _currentListFilter = FilterTypes.All;

		/// <summary>
		/// The filtered housing list that will be displayed.
		/// </summary>
		private List<HousingViewModel> _filteredhousings = new();

		/// <summary>
		/// Returns true if the create housing component is shown.
		/// </summary>
		private bool _isCreatingHousing = false;

        /// <summary>
        /// The ID of the logged in broker if any.
        /// </summary>
        private int? _loggedInBrokerId = null;

        /// <summary>
        /// An element ID to scroll to on next rendering.
        /// </summary>
        private string? _scrollToELementId = null;

        #endregion

        #region Properties

        /// <summary>
        /// The authentication state task. 
        /// </summary>
        [CascadingParameter]
#pragma warning disable CS8618
        private Task<AuthenticationState> AuthenticationStateTask { get; set; }
#pragma warning restore CS8618 

        /// <summary>
        /// Set to true to enable editing controls for brokers. 
        /// </summary>
        [Parameter]
        public bool EnableEditingControls { get; set; } = false;

        /// <summary>
        /// A collection of housing objects to show. 
        /// </summary>
        [Parameter]
        public List<HousingViewModel> Housings { get; set; } = new();

        /// <summary>
        /// Injected JavaScript runtime.
        /// </summary>
        [Inject]
#pragma warning disable CS8618 
		public IJSRuntime JSRuntime {  get; set; }
#pragma warning restore CS8618 

		/// <summary>
		/// Set to true to truncate long descriptions
		/// </summary>
		[Parameter]
        public bool TruncateLongDescriptions { get; set; } = false;

		#endregion

		#region OverriddenMethods

		/// <inheritdoc/>>
		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			await base.OnAfterRenderAsync(firstRender);

			if (_scrollToELementId != null)
			{
				await JSRuntime.InvokeVoidAsync("scrollToElement", _scrollToELementId);
				_scrollToELementId = null;
			}
		}

        /// <inheritdoc/>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            var state = await AuthenticationStateTask;
            
            if (state.User.Claims.Any(x => x.Type == ApplicationUserClaims.BrokerId))
            {
                _loggedInBrokerId = int.Parse(state.User.FindFirst(ApplicationUserClaims.BrokerId)!.Value);
            }            
        }

        /// <inheritdoc/>
        protected override async Task OnParametersSetAsync()
		{
			await base.OnParametersSetAsync();
			RemoveHousingFilter();
		}        

        #endregion

        #region Methods

        /// <summary>
        /// Scrolls to the element.
        /// </summary>
        /// <param name="housing">The housing object to scroll to.</param>
        public void ScrollToElement(HousingViewModel housing)
		{
			_scrollToELementId = $"HousingListItem-{housing.HousingId}";
		}

		/// <summary>
		/// Scrolls to the first element.
		/// </summary>
		public void ScrollToFirstElement()
		{
			if (Housings.Count > 0)
			{
				ScrollToElement(Housings.First());
			}
		}

		/// <summary>
		/// Filters the housing list by broker. 
		/// </summary>
		/// <param name="broker"></param>
		private void FilterByBroker(BrokerViewModel broker)
		{
			_currentListFilter = FilterTypes.Brokers;
			_filteredhousings = Housings.Where(x => x.Broker.BrokerId == broker.BrokerId).ToList();
		}

		/// <summary>
		/// Returns a list of brokers from the housing collection.
		/// </summary>
		/// <returns>A collection of <see cref="BrokerViewModel"/>.</returns>
		private List<BrokerViewModel> GetBrokers()
        {
            return Housings.Select(x => x.Broker)
                    .GroupBy(
                    brokerKey => brokerKey.BrokerId,
                    broker => broker,
                    (brokerId, brokers) => new
                    {
                        Broker = brokers.First()
                    })
                    .Select(x => x.Broker)
                    .OrderBy(x => x.FirstName)
                    .ThenBy(x => x.LastName)
                    .ToList();
        }        

        /// <summary>
        /// Event handler for when the create housing button is clicke.d
        /// </summary>
        private void OnCreateHousingButtonClicked()
        {
            _isCreatingHousing = true;
        }

        /// <summary>
        /// Event handler for the on house created event. 
        /// </summary>
        /// <param name="createdHousing">The new housing object.</param>
        private void OnHousingCreated(HousingViewModel createdHousing)
        {
            Housings.Add(createdHousing);
            _isCreatingHousing = false;
            RemoveHousingFilter();
            ScrollToElement(createdHousing);
        }

        /// <summary>
        /// Event handler for when the housing creation process was cancelled. 
        /// </summary>
        private void OnHousingCreationCancelled()
        {
            _isCreatingHousing = false;
            ScrollToFirstElement();
        }

        /// <summary>
        /// Event handler for when the housing object was deleted.
        /// </summary>
        private void OnHousingDeletedEventHandler(HousingViewModel housing)
        {
            Housings.Remove(housing);
            _filteredhousings.Remove(housing);
        }

		/// <summary>
		/// Event handler for when a list item have transformed into another form. 
		/// </summary>
		/// <param name="housing"></param>
		private void OnListItemTransformed(HousingViewModel housing)
		{
			ScrollToElement(housing);
		}

		/// <summary>
		/// Removes the housing filter.
		/// </summary>
		private void RemoveHousingFilter()
        {
            _currentListFilter = FilterTypes.All;
            _filteredhousings = Housings;
        }

        #endregion
    }
}

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
            NoFilter,
            Brokers
        }

        #endregion

        #region Fields

        /// <summary>
        /// The current filter applied to the list.
        /// </summary>
        private ListFilter _currentFilter = new();

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
			ResetFilter();
		}

        #endregion

        #region Methods

        /// <summary>
        /// Filters the list according to the current filter. 
        /// </summary>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        private void ApplyFilter()
        {
            if (_currentFilter.Type == FilterTypes.Brokers)
            {
                _filteredhousings = Housings.Where(x => x.Broker.BrokerId == _currentFilter.Broker!.BrokerId).ToList();
            }
            else
            {
                _filteredhousings = Housings;
            }

            StateHasChanged();
        }

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
            _currentFilter = new ListFilter(broker);
            ApplyFilter();
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
            ResetFilter();
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
        /// Event handler for when the housing object was edited.
        /// </summary>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        private void OnHousingEditedEventHandler(HousingViewModel housing)
        {
            ApplyFilter();
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
		/// Resets the housing filter.
		/// </summary>
		private void ResetFilter()
        {
            _currentFilter = new ListFilter();
            ApplyFilter();
        }

        #endregion
    }

    /// <summary>
    /// Filter class used in <see cref="HousingList"/> component. 
    /// </summary>
    /// <!-- Author: Jimmie -->
    /// <!-- Co Authors: -->
    class ListFilter
    {
        #region Constructors

        /// <summary>
        /// Constructor to create a filter by broker.
        /// </summary>
        /// <param name="broker">The broker to filter by.</param>
        public ListFilter(BrokerViewModel? broker = null)
        {
            Type = HousingList.FilterTypes.Brokers;
            Broker = broker;
        }

        /// <summary>
        /// Constructor for creating an empty filter. 
        /// </summary>
        public ListFilter()
        {
            Type = HousingList.FilterTypes.NoFilter;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The broker to filter by. 
        /// </summary>
        public BrokerViewModel? Broker { get; private set; }

        /// <summary>
        /// The type of the filter.
        /// </summary>
        public HousingList.FilterTypes Type { get; private set; }        

        #endregion
    }
}

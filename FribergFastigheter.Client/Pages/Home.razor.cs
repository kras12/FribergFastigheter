using Microsoft.AspNetCore.Components;
using FribergFastigheter.Client.Services;

namespace FribergFastigheter.Client.Pages
{
    /// <summary>
    /// The website home page.
    /// </summary>
    /// <!-- Author: Jimmie -->
    /// <!-- Co Authors: -->
    public partial class Home : ComponentBase
    {
        #region Fields

        /// <summary>
        /// Set to true when a housing search operation is running. 
        /// </summary>
        private bool _isSearchingHousings = false;

        #endregion

        #region InjectedServiceProperties
#pragma warning disable CS8618

        [Inject]
        private HousingSearchService HousingSearchService { get; set; }

#pragma warning restore CS8618
        #endregion

        #region Methods

        /// <summary>
        /// Event handler for when the housing search has completed. 
        /// </summary>
        private void OnHousingSearchCompletedEventHandler()
        {
            _isSearchingHousings = false;
        }

        /// <summary>
        /// Event handler for when the housing search has started. 
        /// </summary>
        private void OnHousingSearchStartedEventHandler()
        {
            _isSearchingHousings = true;
        }        

        /// <summary>
        /// Method invoked when the component is ready to start, having received its
        /// initial parameters from its parent in the render tree.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing any asynchronous operation.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        protected async override Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            await HousingSearchService.InitalizeTask;
            HousingSearchService.OnHousingSearchStarted += OnHousingSearchStartedEventHandler;
            HousingSearchService.OnHousingSearchCompleted += OnHousingSearchCompletedEventHandler;
        }

        /// <summary>
        /// Performs a search for housing objects and populates the collection.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        private async Task SearchHousings(int? pageNumber = null)
        {
            await HousingSearchService.SearchHousings(pageNumber);
        }

        #endregion
    }	
}

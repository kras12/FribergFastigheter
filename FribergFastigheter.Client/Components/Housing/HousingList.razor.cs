using FribergFastigheter.Client.Models.Housing;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace FribergFastigheter.Client.Components.Housing
{
    /// <summary>
    /// A component that shows a housing list.
    /// </summary>
    /// <!-- Author: Jimmie -->
    /// <!-- Co Authors: -->
    public partial class HousingList : ComponentBase
    {
        #region Fields

        /// <summary>
        /// Temporary ID variable for which broker firm to use performing actions.
        /// TODO - Replace with data from identity later.
        /// </summary>
        private int _brokerFirmId = 1;

        /// <summary>
        /// The ID of the broker to assign when creating a new housing object.
        /// TODO - Replace with data from identity later.
        /// </summary>
        private int _createHousingBrokerId = 1;

        /// <summary>
        /// Returns true if the create housing component is shown.
        /// </summary>
        private bool _isCreatingHousing = false;

        /// <summary>
        /// An element ID to scroll to on next rendering.
        /// </summary>
        private string? _scrollToELementId = null;        

        #endregion

        #region Properties

        /// <summary>
        /// A collection of housing objects to show. 
        /// </summary>
        [Parameter]
        public List<HousingViewModel> Housings { get; set; } = new();

#pragma warning disable CS8618
        /// <summary>
        /// Injected JavaScript runtime.
        /// </summary>
        [Inject]
        public IJSRuntime JSRuntime {  get; set; }
#pragma warning restore CS8618

        /// <summary>
        /// Set to true to truncate long descriptions
        /// </summary>
        [Parameter]
        public bool TruncateLongDescriptions { get; set; } = false;

        #endregion

        #region Method				

        /// <summary>
        /// Method invoked after each time the component has been rendered interactively
        /// and the UI has finished updating (for example, after elements have been added
        /// to the browser DOM). Any Microsoft.AspNetCore.Components.ElementReference fields
        /// will be populated by the time this runs. This method is not invoked during prerendering
        /// or server-side rendering, because those processes are not attached to any live
        /// browser DOM and are already complete before the DOM is updated. Note that the
        /// component does not automatically re-render after the completion of any returned
        /// System.Threading.Tasks.Task, because that would cause an infinite render loop.
        /// </summary>
        /// <param name="firstRender">Set to true if this is the first time Microsoft.AspNetCore.Components.ComponentBase.OnAfterRender(System.Boolean)
        ///     has been invoked on this component instance; otherwise false.</param>
        /// <returns>A System.Threading.Tasks.Task representing any asynchronous operation.</returns>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (_scrollToELementId != null )
            {
                await JSRuntime.InvokeVoidAsync("scrollToElement", _scrollToELementId);
                _scrollToELementId = null;
            }
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

        #endregion
    }
}

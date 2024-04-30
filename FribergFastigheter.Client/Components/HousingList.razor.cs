using FribergFastigheter.Client.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace FribergFastigheter.Client.Components
{
    /// <summary>
    /// A component that shows a housing list.
    /// </summary>
    /// <!-- Author: Jimmie -->
    /// <!-- Co Authors: -->
    public partial class HousingList : ComponentBase
    {
        #region Fields

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

        #endregion

        #region Method

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

        #endregion
    }
}

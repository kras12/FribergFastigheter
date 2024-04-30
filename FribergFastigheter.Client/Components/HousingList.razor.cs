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

        /// <summary>
        /// Injected JavaScript runtime.
        /// </summary>
        [Inject]
        public IJSRuntime JSRuntime {  get; set; }

        #endregion

        #region Method

        public async Task ScrollToElement(HousingViewModel createdHousing)
        {
            _scrollToELementId = $"HousingListItem-{createdHousing.HousingId}";
        }

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

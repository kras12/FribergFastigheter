using FribergFastigheter.Client.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Net.Http.Headers;

namespace FribergFastigheter.Client.Components
{
    /// <summary>
    /// A component that represents a housing list item. 
    /// </summary>
    /// <!-- Author: Jimmie -->
    /// <!-- Co Authors: -->
    public partial class HousingListItem : ComponentBase
    {
        #region Fields

        /// <summary>
        /// Returns true if the housing object is being edited. 
        /// </summary>
        private bool _isInEditMode = false;

        #endregion

        #region Properties
#pragma warning disable CS8618

        /// <summary>
        /// The ID of list element.
        /// </summary>
        [Parameter]
        public string? ElementId { get; set; } = null;

        /// <summary>
        /// The housing object to show. 
        /// </summary>
        [Parameter]
        public HousingViewModel Housing { get; set; }

        /// <summary>
        /// Event triggers when the element have undergone transformation. 
        /// </summary>
        [Parameter]
        public EventCallback<HousingViewModel> OnTransformed {  get; set; }

#pragma warning restore CS8618
        #endregion

        #region Methods     

        /// <summary>
        /// Event handler for when an editing process for a housing object was cancelled.
        /// </summary>
        /// <param name="housing"></param>
        private void OnCancelEditing(HousingViewModel housing)
        {
            _isInEditMode = false;
            OnTransformed.InvokeAsync(housing);
        }

        /// <summary>
        /// Event handler for when the edit housing button was clicked. 
        /// </summary>
        private void OnEditHousingButtonClicked()
        {
            _isInEditMode = true;
            OnTransformed.InvokeAsync(Housing);
        }

        /// <summary>
        /// Event handler for when a housing object has been edited.
        /// </summary>
        /// <param name="housing"></param>
        private void OnHousingEdited(HousingViewModel housing)
        {
            _isInEditMode = false;
            OnTransformed.InvokeAsync(housing);
        }        

        /// <summary>
        /// Method invoked when the component has received parameters from its parent in
        /// the render tree, and the incoming values have been assigned to properties.
        /// </summary>
        /// <returns>A System.Threading.Tasks.Task representing any asynchronous operation.</returns>
        protected async override Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();

            if (Housing == null)
            {
                throw new ArgumentNullException(nameof(Housing));
            }
        }

        #endregion
    }
}

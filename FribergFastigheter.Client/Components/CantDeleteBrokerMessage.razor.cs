using Microsoft.AspNetCore.Components;
using static FribergFastigheter.Client.Components.ConfirmButton;

namespace FribergFastigheter.Client.Components
{
    public partial class CantDeleteBrokerMessage : ComponentBase
    {
        #region Properies

        /// <summary>
        /// The description of the dialog. 
        /// </summary>
        [Parameter]
        public string Body { get; set; } = "";

        /// <summary>
        /// The title of the dialog.
        /// </summary>
        [Parameter]
        public string Title { get; set; } = "";

        ///// <summary>
        ///// An event that triggers when the user clicks a button to close the dialog. 
        ///// </summary>
        [Parameter]
        public EventCallback OnDialogButtonClicked { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Event handler for when the user clicks any of the dialog buttons. 
        /// </summary>
        /// <param name="result">The result of the user action.</param>
        /// <returns>A <see cref="Task"/>.</returns>
        private Task OnDialogButtonClickedEventHandler()
        {
            return OnDialogButtonClicked.InvokeAsync();
        }

        #endregion
    }
}

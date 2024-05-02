using Microsoft.AspNetCore.Components;

namespace FribergFastigheter.Client.Components
{
    /// <summary>
    /// A component that displays a button that when clicked displays a confirmation dialog where the user can confirm the action.
    /// </summary>
    /// <!-- Author: Jimmie -->
    /// <!-- Co Authors: -->
    public partial class ConfirmButton : ComponentBase
    {
        #region Enums

        /// <summary>
        /// Dialog responses. 
        /// </summary>
        public enum DialogResults
        {
            UserAborted,
            UserConfirmed
        }

        #endregion

        #region Properties

        /// <summary>
        /// The description of the dialog. 
        /// </summary>
        [Parameter]
        public string Body { get; set; } = "";

        /// <summary>
        /// The text of the button. 
        /// </summary>
        [Parameter]
        public string ButtonText { get; set; } = "";

        /// <summary>
        /// The title of the dialog.
        /// </summary>
        [Parameter]
        public string Title { get; set; } = "";

        /// <summary>
        /// An event that triggers when the user clicks a button to close the dialog. 
        /// </summary>
        [Parameter]
        public EventCallback<DialogResults> OnDialogButtonClicked { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Event handler for when the user clicks any of the dialog buttons. 
        /// </summary>
        /// <param name="result">The result of the user action.</param>
        /// <returns>A <see cref="Task"/>.</returns>
        private Task OnDialogButtonClickedEventHandler(DialogResults result)
        {
            return OnDialogButtonClicked.InvokeAsync(result);
        }

        #endregion
    }
}

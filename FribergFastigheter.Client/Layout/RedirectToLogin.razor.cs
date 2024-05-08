using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace FribergFastigheter.Client.Layout
{
    /// <summary>
    /// A component that redirects the user to the login page. 
    /// </summary>
    /// <!-- Author: Jimmie -->
    /// <!-- Co Authors: -->
    public partial class RedirectToLogin : ComponentBase
    {
        #region Properties

        /// <summary>
        /// The injected navigation manager. 
        /// </summary>
        [Inject]
#pragma warning disable CS8618 
        NavigationManager NavigationManager { get; set; }
#pragma warning restore CS8618 

        #endregion

        #region Methods

        /// <summary>
        /// Method invoked when the component is ready to start, having received its initial
        /// parameters from its parent in the render tree.
        /// </summary>
        protected override void OnInitialized()
        {
            NavigationManager.NavigateToLogin("login");
        }

        #endregion
    }
}

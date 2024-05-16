using Blazored.SessionStorage;
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

        /// <summary>
        /// The injected session storage service.
        /// </summary>
        [Inject]
#pragma warning disable CS8618 
        private ISessionStorageService SessionStorageService { get; set; }
#pragma warning restore CS8618 

        /// <summary>
        /// The url to return to after login. 
        /// </summary>
        [Parameter]
        public string? ReturnToUrl { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Method invoked when the component is ready to start, having received its initial
        /// parameters from its parent in the render tree.
        /// </summary>
        protected async override Task OnInitializedAsync()
        {
            if (!string.IsNullOrEmpty(ReturnToUrl))
            {
                await SessionStorageService.SetItemAsStringAsync(LoginButton.RedirectUrlStorageKey, ReturnToUrl);
            }

            NavigationManager.NavigateToLogin("login");
        }

        #endregion
    }
}

using FribergFastigheter.Client.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace FribergFastigheter.Client.Layout
{
    /// <summary>
    /// A component that shows a login button or logout button depending on the authentication state of the user. 
    /// Clicking on the login button redirects the user to the login page, while the logout functionality is handled by the component itself. 
    /// </summary>
    /// <!-- Author: Jimmie -->
    /// <!-- Co Authors: -->
    public partial class LoginButton : ComponentBase
    {
        #region Properties

        /// <summary>
        /// The injected authentication state provider. 
        /// </summary>
        [Inject]
#pragma warning disable CS8618 
        private AuthenticationStateProvider AuthenticationStateProvider { get; set; }
#pragma warning restore CS8618 

        /// <summary>
        /// The injected navigation manager. 
        /// </summary>
        [Inject]
#pragma warning disable CS8618 
        private NavigationManager NavigationManager { get; set; }
#pragma warning restore CS8618 

        #endregion

        #region Methods

        /// <summary>
        /// Logs out the user and redirects the user to the home page. 
        /// </summary>
        /// <returns>A <see cref="Task"/> representing an async operation.</returns>
        public async Task Logout()
        {
            await ((ApiAuthenticationStateProvider)AuthenticationStateProvider).RemoveTokenAsync();
            NavigationManager.NavigateToLogout("/");
        }

        #endregion
    }
}

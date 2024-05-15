using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Http;
using System.Web;

namespace FribergFastigheter.Client.Layout
{
    /// <summary>
    /// A component that redirects the user to an url 
    /// </summary>
    /// <!-- Author: Jimmie -->
    /// <!-- Co Authors: -->
    public partial class RedirectToUrl : ComponentBase
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
        /// The url to redirect to.
        /// </summary>
        [Parameter]
        public string? Url { get; set; } = "";

		#endregion

		#region Methods

		/// <summary>
		/// Method invoked when the component is ready to start, having received its initial
		/// parameters from its parent in the render tree.
		/// </summary>
		protected override void OnInitialized()
		{
            if (string.IsNullOrEmpty(Url))
            {
                throw new ArgumentException("The URL parameter was not set.", nameof(Url));
            }

            NavigationManager.NavigateToLogin($"login");
        }

        #endregion
    }
}

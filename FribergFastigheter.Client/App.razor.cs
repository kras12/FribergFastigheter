using Microsoft.AspNetCore.Components;
using System.Web;

namespace FribergFastigheter.Client
{
	/// <summary>
	/// The application component.
	/// </summary>
	/// <!-- Author: Jimmie -->
	/// <!-- Co Authors: -->
	public partial class App : ComponentBase
    {
        #region Properties

        /// <summary>
        /// The injected navigation manager.
        /// </summary>
        [Inject]
#pragma warning disable CS8618 
        private NavigationManager NavigationManager { get; set; }
#pragma warning restore CS8618

        #endregion
    }
}

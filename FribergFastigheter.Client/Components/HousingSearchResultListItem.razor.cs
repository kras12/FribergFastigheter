using FribergFastigheter.Client.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Net.Http.Headers;

namespace FribergFastigheter.Client.Components
{
    /// <summary>
    /// Component that shows a housing search result list item.
    /// </summary>
    /// <!-- Author: Jimmie -->
    /// <!-- Co Authors: -->
    public partial class HousingSearchResultListItem : ComponentBase
    {
        #region Properties
#pragma warning disable CS8618 
        /// <summary>
        /// The housing object to show. 
        /// </summary>
        [Parameter]
        public HousingViewModel Housing { get; set; }

#pragma warning restore CS8618
        #endregion

        #region Methods

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

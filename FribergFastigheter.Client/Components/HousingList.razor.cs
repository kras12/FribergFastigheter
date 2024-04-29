using FribergFastigheter.Client.Models;
using Microsoft.AspNetCore.Components;

namespace FribergFastigheter.Client.Components
{
    /// <summary>
    /// A component that shows a housing list.
    /// </summary>
    /// <!-- Author: Jimmie -->
    /// <!-- Co Authors: -->
    public partial class HousingList : ComponentBase
    {
        #region Properties

        /// <summary>
        /// A collection of housing objects to show. 
        /// </summary>
        [Parameter]
        public List<HousingViewModel> Housings { get; set; } = new();

        #endregion
    }
}

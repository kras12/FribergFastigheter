using FribergFastigheter.Client.Models;
using Microsoft.AspNetCore.Components;

namespace FribergFastigheter.Client.Components
{
    /// <summary>
    /// Component that shows a housing search listing
    /// </summary>
    /// <!-- Author: Jimmie -->
    /// <!-- Co Authors: -->
    public partial class HousingSearchResultList : ComponentBase
    {
        #region Properties

        [Parameter]
        public HousingSearchResultViewModel SearchResult { get; set; }

        #endregion

        #region Methods        

        #endregion
    }
}

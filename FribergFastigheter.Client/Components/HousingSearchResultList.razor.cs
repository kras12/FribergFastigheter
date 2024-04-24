using FribergFastigheter.Client.Models;
using Microsoft.AspNetCore.Components;

namespace FribergFastigheter.Client.Components
{
    /// <summary>
    /// Component that shows a housing search listing
    /// </summary>
    public partial class HousingSearchResultList : ComponentBase
    {
        #region Properties

        [Parameter]
        public List<HousingViewModel> Housings { get; set; } = new();

        #endregion

        #region Methods        

        #endregion
    }
}

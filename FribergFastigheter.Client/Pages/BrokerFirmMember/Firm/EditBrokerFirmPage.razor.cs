using Microsoft.AspNetCore.Components;

namespace FribergFastigheter.Client.Pages.BrokerFirmMember.Firm
{
    /// <summary>
    /// The edit broker firm page.
    /// </summary>
    public partial class EditBrokerFirmPage : ComponentBase
    {
        #region Properties

        [Parameter]
        public int Id { get; set; }

        #endregion
    }
}

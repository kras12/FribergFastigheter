using Microsoft.AspNetCore.Components;

namespace FribergFastigheter.Client.Pages.BrokerFirmMember.Broker
{
    /// <summary>
    /// The edit broker page.
    /// </summary>
    public partial class EditBrokerPage : ComponentBase
    {
        #region Properties
        
        [Parameter]
        public int Id { get; set; }

		#endregion
	}
}

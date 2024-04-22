using Microsoft.AspNetCore.Components;

namespace FribergFastigheter.Client.Pages.BrokerFirmMember.Broker
{
    /// <summary>
    /// The edit broker housing page.
    /// </summary>
    public partial class EditBrokerHousingPage : ComponentBase
    {
        #region Properties
        
        [Parameter]
        public int Id { get; set; }

		#endregion
	}
}

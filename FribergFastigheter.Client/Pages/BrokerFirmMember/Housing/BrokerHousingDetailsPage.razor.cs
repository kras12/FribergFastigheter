using Microsoft.AspNetCore.Components;

namespace FribergFastigheter.Client.Pages.BrokerFirmMember.Broker
{
    /// <summary>
    /// The broker housing details page.
    /// </summary>
    public partial class BrokerHousingDetailsPage : ComponentBase
    {
        #region Properties
        
        [Parameter]
        public int Id { get; set; }

		#endregion
	}
}

using Microsoft.AspNetCore.Components;
using System.ComponentModel;

namespace FribergFastigheter.Client.Pages.BrokerFirmMember
{
    /// <summary>
    /// The home page of the broker back office. 
    /// </summary>
    public partial class BrokerHomePage : Component
    {
        #region Properties
        
        [Parameter]
        public int Id { get; set; }

		#endregion
	}
}

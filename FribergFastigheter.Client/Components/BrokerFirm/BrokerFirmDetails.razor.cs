using FribergFastigheter.Client.Models;
using Microsoft.AspNetCore.Components;

namespace FribergFastigheter.Client.Components.BrokerFirm
{
    public partial class BrokerFirmDetails : ComponentBase
    {
        #region Properties
        [Parameter]
        public BrokerFirmViewModel BrokerFirm { get; set; }

        #endregion

        #region Constructors

        public BrokerFirmDetails()
        {

        }

        #endregion

        #region Methods

        #endregion
    }
}

using FribergFastigheter.Client.Models;
using Microsoft.AspNetCore.Components;

namespace FribergFastigheter.Client.Components
{
    public partial class BrokerList : ComponentBase
    {
        #region Properties

        [Parameter]
        public List<BrokerViewModel> Brokers { get; set; }
        [Parameter]
        public BrokerFirmSummaryViewModel BrokerFirm { get; set; }

        #endregion

        #region Constructors

        public BrokerList()
        {
                
        }

        #endregion

        #region Methods

        #endregion
    }
}

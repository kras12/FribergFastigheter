using Microsoft.AspNetCore.Components;
using System.ComponentModel;

namespace FribergFastigheter.Client.Pages.Broker
{
    public partial class BrokerHome : Component
    {
        [Parameter]
        public int Id { get; set; }
    }

}

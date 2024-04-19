using Microsoft.AspNetCore.Components;
using System.ComponentModel;

namespace FribergFastigheter.Client.Pages
{
    public partial class BrokerHome : Component
    {
        [Parameter]
        public int Id { get; set; }
    }

}

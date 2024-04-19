using Microsoft.AspNetCore.Components;

namespace FribergFastigheter.Client.Pages
{
    public partial class BrokerFirm: ComponentBase
    {
        [Parameter]
        public int Id { get; set; }
    }
}

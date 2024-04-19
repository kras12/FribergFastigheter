using Microsoft.AspNetCore.Components;

namespace FribergFastigheter.Client.Pages
{
    public partial class Broker: ComponentBase
    {
        [Parameter]
        public int Id { get; set; }
    }
}

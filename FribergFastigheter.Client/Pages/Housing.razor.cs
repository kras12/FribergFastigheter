using Microsoft.AspNetCore.Components;

namespace FribergFastigheter.Client.Pages
{
    public partial class Housing : ComponentBase
    {
        [Parameter]
        public int Id { get; set; }
    }
}

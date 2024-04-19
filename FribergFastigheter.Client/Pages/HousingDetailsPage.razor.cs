using Microsoft.AspNetCore.Components;

namespace FribergFastigheter.Client.Pages
{
    /// <summary>
    /// The housing details page.
    /// </summary>
    public partial class HousingDetailsPage : ComponentBase
    {
		#region Properties

		[Parameter]
		public int Id { get; set; }

		#endregion
	}
}

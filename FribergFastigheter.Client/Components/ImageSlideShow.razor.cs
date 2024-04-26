using FribergFastigheter.Client.Models;
using Microsoft.AspNetCore.Components;

namespace FribergFastigheter.Client.Components
{
	/// <summary>
	/// Component for displaying image slide shows.
	/// </summary>
	public partial class ImageSlideShow
	{
		#region Properties

		[Parameter]
		public List<ImageSlideViewModel> Images { get; set; } = new();

		#endregion
	}
}

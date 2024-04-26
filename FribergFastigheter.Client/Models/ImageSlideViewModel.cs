using System.ComponentModel;
using FribergFastigheter.Client.Components;

namespace FribergFastigheter.Client.Models
{
	/// <summary>
	/// A view model class designed to be used with the image slideshow component <see cref="ImageSlideShow"/>.
	/// </summary>
	public class ImageSlideViewModel
    {
        #region Constructors

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="url">The url for the image.</param>
		/// <param name="fileName">The filename for the image.</param>
		/// <param name="imageId">The ID for the image.</param>
		/// <param name="linksToUrl">An optional link to another page.</param>
		/// <param name="imageCaption">An optional image caption.</param>
		public ImageSlideViewModel(string url, string fileName = "", int? imageId = null, string? imageCaption = null, string? linksToUrl = null)
        {
            FileName = fileName;
            Url = url;
            ImageId = imageId;
			LinksToUrl = linksToUrl;
            ImageCaption = imageCaption;
        }

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="image">The image to model.</param>
		/// <param name="imageId">The ID for the image.</param>
		/// <param name="linksToUrl">An optional link to another page.</param>
		public ImageSlideViewModel(ImageViewModel image, string? imageCaption = null, string? linksToUrl = null)
        {
            FileName = image.FileName;
            Url = image.Url;
            ImageId= image.ImageId;
			LinksToUrl = linksToUrl;
			ImageCaption = imageCaption;
		}

        #endregion

        #region Properties

        /// <summary>
        /// The filename for the image.
        /// </summary>
        [DisplayName("File Name")]
        public string FileName { get; } = "";

        /// <summary>
        /// An optional image caption. 
        /// </summary>
        public string? ImageCaption { get; set; } = null;

        /// <summary>
        /// Returns true if there is an image caption.
        /// </summary>
        public bool HaveCaption
        {
            get
            {
                return !string.IsNullOrEmpty(ImageCaption);
            }
        }

        /// <summary>
        /// The ID for the image.
        /// </summary>
        [DisplayName("Image ID")]
        public int? ImageId { get; }

		/// <summary>
		/// An optional link to another page.
		/// </summary>
		[DisplayName("Link")]
		public string? LinksToUrl { get; set;  }

        /// <summary>
        /// The url for the image.
        /// </summary>
        [DisplayName("Url")]
        public string Url { get; } = "";


        #endregion
    }
}

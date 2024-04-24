using FribergFastigheter.Shared.Enums;

namespace FribergFastigheter.Client.Models
{
    /// <summary>
    /// A view model class that represents an image.
    /// </summary>
    /// <!-- Author: Jimmie -->
    /// <!-- Co Authors: -->
    public class ImageViewModel
    {
        #region Properties

        /// <summary>
        /// The filename of the image.
        /// </summary>
        public string FileName { get; set; } = "";

        /// <summary>
        /// The ID of the image.
        /// </summary>
        public int ImageId { get; set; }

        /// <summary>
        /// The image type.
        /// </summary>

        public ImageTypes ImageType { get; set; }
        /// <summary>
        /// The url of the image.
        /// </summary>
        public string Url { get; set; } = "";

        #endregion
    }
}

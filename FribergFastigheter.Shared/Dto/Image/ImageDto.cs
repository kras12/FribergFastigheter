using FribergFastigheter.Shared.Enums;

namespace FribergFastigheter.Shared.Dto.Image
{
	/// <summary>
	/// DTO class for images.
	/// </summary>
	/// <!-- Author: Jimmie -->
	/// <!-- Co Authors: -->
	public class ImageDto
    {
        #region Properties

        /// <summary>
        /// The filename of the image.
        /// </summary>
        public string FileName { get; set; } = "";

        /// <summary>
        /// The image type.
        /// </summary>
        public ImageTypes ImageType { get; set; }

        /// <summary>
        /// The ID of the image.
        /// </summary>
        public int ImageId { get; set; }

        /// <summary>
        /// The url of the image.
        /// </summary>
        public string Url { get; set; } = "";

        #endregion
    }
}

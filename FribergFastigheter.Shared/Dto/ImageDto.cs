using FribergFastigheter.Shared.Enums;

namespace FribergFastigheter.Shared.Dto
{
    public class ImageDto
    {
        #region Properties

        /// <summary>
        /// A base64 string representation of the image file data.
        /// </summary>
        public string Base64 { get; set; } = "";

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

        #endregion
    }
}

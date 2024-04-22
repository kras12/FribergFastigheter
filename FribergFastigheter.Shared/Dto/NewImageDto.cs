using FribergFastigheter.Shared.Enums;

namespace FribergFastigheter.Shared.Dto
{
    /// <summary>
    /// A DTO class that represents a new image.
    /// </summary>
    /// <!-- Author: Jimmie -->
    /// <!-- Co Authors: -->
    public class NewImageDto
    {
        #region Properties

        /// <summary>
        /// A base64 string representation of the image file data.
        /// </summary>
        public string Base64 { get; set; } = "";

        /// <summary>
        /// The image type.
        /// </summary>
        public ImageTypes ImageType { get; set; }

        #endregion
    }
}

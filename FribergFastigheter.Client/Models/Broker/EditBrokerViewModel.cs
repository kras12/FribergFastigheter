using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using FribergFastigheter.Client.Models.Image;

namespace FribergFastigheter.Client.Models.Broker
{
    /// <summary>
    /// A view model class that represents a broker.
    /// </summary>
    /// <!-- Author: Jimmie -->
    /// <!-- Co Authors: -->
    public class EditBrokerViewModel : ViewModelBase
    {
        #region Properties

        /// <summary>
        /// The ID of the broker.
        /// </summary>
        public int BrokerId { get; set; }

        /// <summary>
        /// The description of the broker.
        /// </summary>
        public string Description { get; set; } = "";

        /// <summary>
        /// The email of the broker.
        /// </summary>
        [Required]
        [EmailAddress]
        public string Email { get; set; } = "";
        /// <summary>
        /// The first name of the broker.
        /// </summary>
        [Required]
        [RegularExpression(NameValidationExpression, ErrorMessage = NameValidationErrorMessage)]
        public string FirstName { get; set; } = "";

        /// <summary>
        /// The last name of the broker.
        /// </summary>
        [Required]
        [RegularExpression(NameValidationExpression, ErrorMessage = NameValidationErrorMessage)]
        public string LastName { get; set; } = "";

        /// <summary>
        /// The phone number of the broker.
        /// </summary>
        public string PhoneNumber { get; set; } = "";

        /// <summary>
        /// The broker profile image.
        /// </summary>
        public ImageViewModel? ProfileImage { get; set; }

        /// <summary>
        /// Returns the url of the profile image or a placeholder if no image exists. 
        /// </summary>
        public string ProfileImageOrPlaceholder
        {
            get
            {
                if (ProfileImage == null)
                {
                    return "/Graphics/profile-image-placeholder.jpg";
                }
                return ProfileImage.Url;
            }
        }

        #endregion
    }
}

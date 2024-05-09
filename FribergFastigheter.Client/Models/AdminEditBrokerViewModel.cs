using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FribergFastigheter.Client.Models
{
    /// <summary>
    /// A view model class for when an administrator is editing a broker. 
    /// </summary>
    /// <!-- Author: Jimmie -->
    /// <!-- Co Authors: -->
    public class AdminEditBrokerViewModel
    {
        #region Properties

        /// <summary>
        /// The firm that the broker belongs to.
        /// </summary>
        public BrokerFirmSummaryViewModel BrokerFirm { get; set; } = new();

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
        public string Email { get; set; } = "";
        /// <summary>
        /// The first name of the broker.
        /// </summary>
        public string FirstName { get; set; } = "";

        /// <summary>
        /// The last name of the broker.
        /// </summary>
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

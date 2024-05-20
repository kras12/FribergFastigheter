using FribergFastigheter.Client.Models.BrokerFirm;
using FribergFastigheter.Client.Models.Image;
using FribergFastigheter.Client.Services.AuthorizationHandlers.Broker.Data;
using FribergFastigheter.Shared.Dto;
using FribergFastigheter.Shared.Services.AuthorizationHandlers.Broker.Data;
using System.ComponentModel.DataAnnotations;

namespace FribergFastigheter.Client.Models.Broker
{
    public class BrokerViewModel : ViewModelBase, IEditBrokerPreAuthorizationData, IDeleteBrokerPreAuthorizationData, IDeleteBrokerAuthorizationData
    {
        #region Constructors

        /// <summary>
        /// A constructor.
        /// </summary>
        public BrokerViewModel()
        {

        }

        #endregion

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
        /// The email of the broker.
        /// </summary>
        public string Email { get; set; } = "";

        /// <summary>
        /// The description of the broker.
        /// </summary>
        public string Description { get; set; } = "";

        /// <summary>
        /// The number of houses the broker manages.
        /// </summary>
        public int? HousingCount { get; set; }

        /// <summary>
        /// The first name of the broker.
        /// </summary>
        public string FirstName { get; set; } = "";

        /// <summary>
        /// The full name of the broker.
        /// </summary>
        public string FullName
        {
            get
            {
                return $"{FirstName} {LastName}";
            }
        }

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
        public ImageViewModel? ProfileImage { get; set; } = null;

        /// <summary>
        /// Returns the url for the profile image or a placeholder if no image exists.
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

        /// <summary>
        /// An optional URL linking to the housing object.
        /// </summary>
        public string? Url { get; set; } = null;

        #endregion

        #region IEditHousingPreAuthorizationData

        /// <summary>
        /// The broker firm ID
        /// </summary>
        int IEditBrokerPreAuthorizationData.ExistingBrokerBrokerFirmId
        {
            get
            {
                return BrokerFirm.BrokerFirmId;
            }
        }

        /// <summary>
        /// The broker ID
        /// </summary>
        int IEditBrokerPreAuthorizationData.ExistingBrokerBrokerId
        {
            get
            {
                return BrokerId;
            }
        }

        #endregion

        #region IDeleteBrokerAuthorizationData

        /// <summary>
        /// The broker firm ID
        /// </summary>
        int IDeleteBrokerAuthorizationData.ExistingBrokerBrokerFirmId
        {
            get
            {
                return BrokerFirm.BrokerFirmId;
            }
        }

        #endregion

        #region IDeleteBrokerPreAuthorizationData

        /// <summary>
        /// The broker firm ID
        /// </summary>
        int IDeleteBrokerPreAuthorizationData.ExistingBrokerBrokerFirmId
        {
            get
            {
                return BrokerFirm.BrokerFirmId;
            }
        }

        #endregion
    }
}

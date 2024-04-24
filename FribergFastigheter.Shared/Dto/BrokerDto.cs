using System.ComponentModel.DataAnnotations;

namespace FribergFastigheter.Shared.Dto
{
    /// <summary>
    /// A DTO class that represents a broker.
    /// </summary>
    /// <!-- Author: Jimmie -->
    /// <!-- Co Authors: -->
    public class BrokerDto
    {
        #region Properties

        /// <summary>
        /// The firm that the broker belongs to.
        /// </summary>
        public BrokerFirmDto BrokerFirm { get; set; }

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
        public ImageDto? ProfileImage { get; set; } = null;

        #endregion
    }
}

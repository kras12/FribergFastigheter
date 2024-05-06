using System.ComponentModel.DataAnnotations;

namespace FribergFastigheter.Shared.Dto
{
    /// <summary>
    /// A DTO class that represents a broker.
    /// </summary>
    /// <!-- Author: Marcus -->
    /// <!-- Co Authors: -->
    public class RegisterBrokerDto
    {
        #region Properties

        /// <summary>
        /// The firm that the broker belongs to.
        /// TODO - Remove property
        /// </summary>
        public int BrokerFirmId { get; set; }

        /// <summary>
        /// The username/email of the broker.
        /// </summary>
        [Required]
        public string Email { get; set; } = "";

        /// <summary>
        /// The first name of the broker.
        /// </summary>
        [Required]
        public string FirstName { get; set; } = "";

        /// <summary>
        /// The last name of the broker.
        /// </summary>
        [Required]
        public string LastName { get; set; } = "";

        /// <summary>
        /// The phone number of the broker.
        /// </summary>
        [Required]
        public string PhoneNumber { get; set; } = "";

        /// <summary>
        /// The description of the broker.
        /// </summary>
        public string Description { get; set; } = "";

        /// <summary>
        ///The password for the broker.
        /// </summary>
        [Required]
        public string Password { get; set; } = "";

        #endregion
    }
}

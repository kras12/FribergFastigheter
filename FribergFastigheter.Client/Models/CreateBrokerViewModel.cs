using System.ComponentModel.DataAnnotations;

namespace FribergFastigheter.Client.Models
{
    /// <summary>
    /// A view model class that represents a broker.
    /// </summary>
    /// <!-- Author: Jimmie -->
    /// <!-- Co Authors: -->
    public class CreateBrokerViewModel
    {
        #region Properties

        /// <summary>
        /// The firm that the broker belongs to.
        /// </summary>
        public int BrokerFirmId { get; set; }

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
        /// The description of the broker.
        /// </summary>
        public string Description { get; set; } = "";

        #endregion
    }
}

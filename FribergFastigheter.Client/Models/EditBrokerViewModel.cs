using FribergFastigheter.Shared.Dto;
using System.ComponentModel.DataAnnotations;

namespace FribergFastigheter.Client.Models
{
    public class EditBrokerViewModel
    {
        #region Constructors

    /// <summary>
        /// A constructor.
    /// </summary>
        public EditBrokerViewModel()
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

		#endregion
	}
}

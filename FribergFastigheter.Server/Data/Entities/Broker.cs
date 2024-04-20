using FribergFastigheter.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace FribergFastigheterApi.Data.Entities
{
	/// <summary>
	/// An entity class that represent a broker.
	/// </summary>
	/// <!-- Author: Jimmie -->
	/// <!-- Co Authors: -->
	public class Broker
	{
        #region Constructors

		/// <summary>
		/// Constructor.
		/// </summary>
        public Broker()
        {
            
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="firstName">The first name of the broker.</param>
        /// <param name="lastName">The last name of the broker.</param>
        /// <param name="email">The email of the broker.</param>
        /// <param name="phoneNumber">The phonenumber of the broker.</param>
        /// <param name="description">The description of the broker.</param>
        /// <param name="profileImage">The profile image of the broker.</param>
        /// <exception cref="ArgumentException"></exception>
        public Broker(string firstName, string lastName, string email, string phoneNumber, BrokerFirm brokerFirm, string description = "", Image? profileImage = null)
		{
			#region Checks

			if (string.IsNullOrEmpty(firstName))
			{
				throw new ArgumentException($"The value of parameter '{nameof(firstName)}' can't be null or empty.", nameof(firstName));
			}

			if (string.IsNullOrEmpty(lastName))
			{
				throw new ArgumentException($"The value of parameter '{nameof(lastName)}' can't be null or empty.", nameof(lastName));
			}

			if (string.IsNullOrEmpty(email))
			{
				throw new ArgumentException($"The value of parameter '{nameof(email)}' can't be null or empty.", nameof(email));
			}

			if (string.IsNullOrEmpty(phoneNumber))
			{
				throw new ArgumentException($"The value of parameter '{nameof(phoneNumber)}' can't be null or empty.", nameof(phoneNumber));
			}

			if (brokerFirm == null)
			{
				throw new ArgumentNullException(nameof(brokerFirm), $"The value of parameter '{nameof(brokerFirm)}' can't be null.");
			}

			#endregion

			FirstName = firstName;
			LastName = lastName;
			Email = email;
			PhoneNumber = phoneNumber;
			BrokerFirm = brokerFirm;
			Description = description;
			ProfileImage = profileImage;
		}

		#endregion

		#region Properties

		/// <summary>
		/// The firm that the broker belongs to.
		/// </summary>
		[Required]
		public BrokerFirm BrokerFirm { get; set; }

		/// <summary>
		/// The ID of the broker.
		/// </summary>
		[Key]
		public int BrokerId { get; set; }

        /// <summary>
        /// The description of the broker.
        /// </summary>
        [Required]
        public string Description { get; set; } = "";

        /// <summary>
        /// The email of the broker.
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
		/// The broker profile image.
		/// </summary>
		public Image? ProfileImage { get; set; } = null;

		#endregion
	}
}

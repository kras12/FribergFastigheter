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
		/// <exception cref="ArgumentException"></exception>
		public Broker(string firstName, string lastName, BrokerFirm brokerFirm)
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

			if (brokerFirm == null)
			{
				throw new ArgumentNullException(nameof(brokerFirm), $"The value of parameter '{nameof(brokerFirm)}' can't be null.");
			}

			#endregion

			FirstName = firstName;
			LastName = lastName;
			BrokerFirm = brokerFirm;
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
		/// The first name of the broker.
		/// </summary>
		public string FirstName { get; set; } = "";

		/// <summary>
		/// The last name of the broker.
		/// </summary>
		public string LastName { get; set; } = "";

		#endregion
	}
}

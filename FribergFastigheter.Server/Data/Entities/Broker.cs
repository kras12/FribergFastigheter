using System.ComponentModel.DataAnnotations;

namespace FribergFastigheter.Server.Data.Entities
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
		/// <param name="brokerFirm">The firm that the broker belongs to.</param>
		/// <param name="description">The description of the broker.</param>
		/// <param name="profileImage">The profile image of the broker.</param>
		///<param name="user">The user associated with the broker. </param>
		/// <exception cref="ArgumentException"></exception>
		public Broker(BrokerFirm brokerFirm, string description = "", Image? profileImage = null, ApplicationUser? user = null)
		{
            #region Checks

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), $"The value of parameter '{nameof(user)}' can't be null.");
            }


            if (brokerFirm == null)
			{
				throw new ArgumentNullException(nameof(brokerFirm), $"The value of parameter '{nameof(brokerFirm)}' can't be null.");
			}

			#endregion

			BrokerFirm = brokerFirm;
			Description = description;
			ProfileImage = profileImage;

			if (user != null)
			{
				User = user;
			}
		}

		#endregion

		#region Properties

		/// <summary>
		/// The user associated with the broker.
		/// </summary>
		[Required]
		public ApplicationUser User { get; set; }

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
		/// The broker profile image.
		/// </summary>
		public Image? ProfileImage { get; set; } = null;

		#endregion
	}
}

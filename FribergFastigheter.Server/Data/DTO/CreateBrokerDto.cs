using FribergFastigheterApi.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace FribergFastigheter.Server.Data.DTO
{
	/// <summary>
	/// A DTO class that represents a broker.
	/// </summary>
	/// <!-- Author: Marcus -->
	/// <!-- Co Authors: -->
	public class CreateBrokerDto
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
		/// The broker profile image.
		/// </summary>
		public ImageDto? ProfileImage { get; set; } = null;

		#endregion
	}
}

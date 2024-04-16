using FribergFastigheterApi.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace FribergFastigheter.Server.Data.DTO
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
		/// The full name of the broker.
		/// </summary>
		public string FullName { get; set; } = "";

		/// <summary>
		/// The email of the broker.
		/// </summary>
		public string Email { get; set; } = "";

		/// <summary>
		/// The phone number of the broker.
		/// </summary>
		public string PhoneNumber { get; set; } = "";

		/// <summary>
		/// The broker profile image.
		/// </summary>
		public string? ProfileImage { get; set; } = null;

		#endregion
	}
}

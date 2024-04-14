using FribergFastigheterApi.Data.Entities;

namespace FribergFastigheterApi.HelperClasses.Data
{
	/// <summary>
	/// A class that stores serialized seed data for housing objects. 
	/// </summary>
	/// <!-- Author: Jimmie -->
	/// <!-- Co Authors: -->
	public class HousingSeedDataRow
    {
		/// <summary>
		/// The address of the housing object.
		/// </summary>
		public string Address { get; set; } = "";

		/// <summary>
		/// The name of the broker.
		/// </summary>
		public string Broker { get; set; } = "";

		/// <summary>
		/// The name of the broker firm.
		/// </summary>
		public string BrokerFirm { get; set; } = "";

		/// <summary>
		/// The description of the housing object.
		/// </summary>
		public string Description { get; set; } = "";

		/// <summary>
		/// An image associated with the housing object.
		/// </summary>
		public string Image { get; set; } = "";

		/// <summary>
		/// The key for extra housing data.
		/// </summary>
		public string Key { get; set; } = "";

		/// <summary>
		/// The area and municipality of the housing object.
		/// </summary>
		public string Location { get; set; } = "";

		/// <summary>
		/// The URL of the housing object page.
		/// </summary>
		public string PageURL { get; set; } = "";

		/// <summary>
		/// The price of the housing object.
		/// </summary>
		public string Price { get; set; } = "";

		/// <summary>
		/// The value for extra housing data. 
		/// </summary>
		public string Value { get; set; } = "";	
    }
}

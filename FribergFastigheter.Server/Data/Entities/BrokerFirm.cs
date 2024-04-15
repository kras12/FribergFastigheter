using System.ComponentModel.DataAnnotations;

namespace FribergFastigheterApi.Data.Entities
{
	/// <summary>
	/// An entity class that represent a broker firm.
	/// </summary>
	/// <!-- Author: Jimmie -->
	/// <!-- Co Authors: -->
	public class BrokerFirm
	{
		#region Constructors

		/// <summary>
		/// Constructor.
		/// </summary>
		public BrokerFirm()
		{

		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="name">The name of the broker firm.</param>
		/// <exception cref="ArgumentException"></exception>
		public BrokerFirm(string name)
		{
			#region Checks

			if (string.IsNullOrEmpty(name))
			{
				throw new ArgumentException($"The value of parameter '{nameof(name)}' can't be null or empty.", nameof(name));
			}

			#endregion

			Name = name;
		}

		#endregion

		#region Properties

		/// <summary>
		/// The ID of the broker firm.
		/// </summary>
		[Key]
		public int BrokerFirmId { get; set; }

		/// <summary>
		/// The brokers working at the firm.
		/// </summary>
		public List<Broker> Brokers { get; set; } = new();

		/// <summary>
		/// The description of the broker firm. 
		/// </summary>
		public string Description { get; set; } = "";

		/// <summary>
		/// The file name of the logotype.
		/// </summary>
		public Image? Logotype { get; set; }

		/// <summary>
		/// The name of the broker firm.
		/// </summary>
		public string Name { get; set; } = "";

		#endregion
	}
}

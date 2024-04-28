using FribergFastigheter.Shared.Dto;
using System.ComponentModel.DataAnnotations;

namespace FribergFastigheter.Client.Models
{
    /// <summary>
    /// A view model class that represents a broker firm.
    /// </summary>
    /// <!-- Author: Jimmie -->
    /// <!-- Co Authors: -->
    public class BrokerFirmViewModel : BrokerFirmSummaryViewModel
    {
		#region Properties

		/// <summary>
		/// The brokers at the firm.
		/// </summary>
		public List<BrokerViewModel> Brokers { get; set; } = new();

		#endregion
	}
}

using System.ComponentModel.DataAnnotations;

namespace FribergFastigheter.Shared.Dto
{
    /// <summary>
    /// A DTO class that represents a broker firm.
    /// </summary>
    /// <!-- Author: Jimmie -->
    /// <!-- Co Authors: -->
    public class BrokerFirmDto : BrokerFirmSummaryDto
    {
        #region Properties

		/// <summary>
		/// The brokers at the firm.
		/// </summary>
		public List<BrokerDto> Brokers { get; set; } = new();

        #endregion
    }
}

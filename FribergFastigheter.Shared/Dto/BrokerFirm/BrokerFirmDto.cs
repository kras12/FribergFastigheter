using System.ComponentModel.DataAnnotations;
using FribergFastigheter.Shared.Dto.Broker;

namespace FribergFastigheter.Shared.Dto.BrokerFirm
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

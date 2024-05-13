using System.ComponentModel.DataAnnotations;
using FribergFastigheter.Shared.Dto.Image;

namespace FribergFastigheter.Shared.Dto.BrokerFirm
{
    /// <summary>
    /// A DTO class that represents a broker firm.
    /// </summary>
    /// <!-- Author: Jimmie -->
    /// <!-- Co Authors: -->
    public class BrokerFirmSummaryDto
    {
        #region Properties

        /// <summary>
        /// The ID of the broker firm.
        /// </summary>
        public int BrokerFirmId { get; set; }

        /// <summary>
        /// The description of the broker firm. 
        /// </summary>
        public string Description { get; set; } = "";

        /// <summary>
        /// The file name of the logotype.
        /// </summary>
        public ImageDto? Logotype { get; set; }

        /// <summary>
        /// The name of the broker firm.
        /// </summary>
        public string Name { get; set; } = "";

        #endregion
    }
}

using System.ComponentModel.DataAnnotations;
using FribergFastigheter.Client.Models.Image;

namespace FribergFastigheter.Client.Models.BrokerFirm
{
    /// <summary>
    /// A view model class that represents a broker firm.
    /// </summary>
    /// <!-- Author: Jimmie -->
    /// <!-- Co Authors: -->
    public class BrokerFirmSummaryViewModel
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
        /// The url of the logotype.
        /// </summary>
        public ImageViewModel? Logotype { get; set; }

        /// <summary>
        /// The name of the broker firm.
        /// </summary>
        public string Name { get; set; } = "";

        /// <summary>
        /// An optional URL linking to the housing object.
        /// </summary>
        public string? Url { get; set; } = null;

        #endregion
    }
}

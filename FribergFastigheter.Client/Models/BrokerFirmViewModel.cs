using System.ComponentModel.DataAnnotations;

namespace FribergFastigheter.Client.Models
{
    /// <summary>
    /// A view model class that represents a broker firm.
    /// </summary>
    /// <!-- Author: Jimmie -->
    /// <!-- Co Authors: -->
    public class BrokerFirmViewModel
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
        public ImageViewModel? Logotype { get; set; }

        /// <summary>
        /// The name of the broker firm.
        /// </summary>
        public string Name { get; set; } = "";

        #endregion
    }
}

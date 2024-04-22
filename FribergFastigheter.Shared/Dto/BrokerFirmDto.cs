﻿using System.ComponentModel.DataAnnotations;

namespace FribergFastigheter.Shared.Dto
{
    /// <summary>
    /// A DTO class that represents a broker firm.
    /// </summary>
    /// <!-- Author: Jimmie -->
    /// <!-- Co Authors: -->
    public class BrokerFirmDto
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

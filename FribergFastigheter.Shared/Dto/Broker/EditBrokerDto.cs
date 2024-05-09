using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FribergFastigheter.Shared.Dto.Broker
{
    /// <summary>
    /// A DTO class that contains data for when a broker is editing themselves.
    /// </summary>
    /// <!-- Author: Marcus -->
    /// <!-- Co Authors: -->
    public class EditBrokerDto
    {
        #region Properties

        /// <summary>
        /// The ID of the broker.
        /// </summary>
        public int BrokerId { get; set; }

        /// <summary>
        /// The description of the broker.
        /// </summary>
        public string Description { get; set; } = "";

        #endregion
    }
}


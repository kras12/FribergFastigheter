using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FribergFastigheter.Shared.Dto
{
    /// <summary>
    /// A DTO class that contains data for a created broker.
    /// </summary>
    /// <!-- Author: Jimmie, Marcus -->
    /// <!-- Co Authors: -->
    public class CreatedBrokerDto
    {
        #region Properties

        /// <summary>
        /// The broker ID.
        /// </summary>
        public int BrokerId { get; set; }

        /// <summary>
        /// The email for the broker. 
        /// </summary>
        public string Email { get; set; } = "";

        /// <summary>
        /// The first name for the broker. 
        /// </summary>
        public string FirstName { get; set; } = "";

        /// <summary>
        /// The last name for the broker. 
        /// </summary>
        public string LastName { get; set; } = "";

        /// <summary>
        /// The user name for the broker. 
        /// </summary>
        public string UserName { get; set; } = "";

        #endregion

    }
}

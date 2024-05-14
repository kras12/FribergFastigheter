using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FribergFastigheter.Shared.Services.AuthorizationHandlers.Broker.Data
{
    /// <summary>
    /// Data storage class designed to be used with the <see cref="ManageBrokerAuthorizationHandler"/> to enable authorization for creating new housing objects.
    /// </summary>
    /// <!-- Author: Jimmie, Marcus -->
    /// <!-- Co Authors: -->
    public class CreateBrokerAuthorizationData : ICreateBrokerAuthorizationData
    {

        #region Propterties

        /// <summary>
        /// The new broker ID.
        /// </summary>
        public int? NewBrokerBrokerId { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="newBrokerBrokerId">The new broker ID.</param>
        public CreateBrokerAuthorizationData(int? newBrokerBrokerId)
        {
            NewBrokerBrokerId = newBrokerBrokerId;
        }

        #endregion
    }
}

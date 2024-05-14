using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FribergFastigheter.Shared.Services.AuthorizationHandlers.Broker.Data
{
    /// <summary>
    /// Data storage class designed to be used with the <see cref="ManageBrokerAuthorizationHandler"/> to enable authorization for deleting housing objects.
    /// </summary>
    /// <!-- Author: Jimmie, Marcus -->
    /// <!-- Co Authors: -->
    public class DeleteBrokerAuthorizationData : IDeleteBrokerAuthorizationData
    {
        #region Properties

        /// <summary>
        /// The existing broker brokerfirm ID.
        /// </summary>
        public int ExistingBrokerBrokerFirmId { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="existingBrokerBrokerFirmId">The existing broker brokerfirm ID.</param>
        public DeleteBrokerAuthorizationData(int existingBrokerBrokerFirmId)
        {
            ExistingBrokerBrokerFirmId = existingBrokerBrokerFirmId;
        }

        #endregion
    }
}

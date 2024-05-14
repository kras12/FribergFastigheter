using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FribergFastigheter.Shared.Services.AuthorizationHandlers.Broker.Data
{
    /// <summary>
    /// Interface designed to be used with the <see cref="ManageBrokerAuthorizationHandler"/> to enable authorization for creating new housing objects.
    /// </summary>
    /// <!-- Author: Jimmie, Marcus -->
    /// <!-- Co Authors: -->
    public interface IDeleteBrokerAuthorizationData
    {

        /// <summary>
        /// The existing broker brokerfirm ID.
        /// </summary>
        public int ExistingBrokerBrokerFirmId { get; }

    }
}

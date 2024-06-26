﻿using FribergFastigheter.Client.Services.AuthorizationHandlers.Broker;

namespace FribergFastigheter.Client.Services.AuthorizationHandlers.Broker.Data
{
    /// <summary>
    /// Interface designed to be used with the <see cref="ManageBrokerPreAuthorizationHandler"/> to enable authorization for deleting broker objects.
    /// </summary>
    /// <!-- Author: Jimmie, Marcus -->
    /// <!-- Co Authors: -->
    public interface IDeleteBrokerPreAuthorizationData
    {
        /// <summary>
        /// The existing housing broker firm ID.
        /// </summary>
        public int ExistingBrokerBrokerFirmId { get; }


    }
}

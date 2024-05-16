using FribergFastigheter.Client.Services.AuthorizationHandlers.Broker;

namespace FribergFastigheter.Client.Services.AuthorizationHandlers.Broker.Data
{
    /// <summary>
    /// Interface designed to be used with the <see cref="ManageBrokerPreAuthorizationHandler"/> to enable authorization for editing broker objects.
    /// </summary>
    /// <!-- Author: Jimmie, Marcus -->
    /// <!-- Co Authors: -->
    public interface IEditBrokerPreAuthorizationData
    {
        /// <summary>
        /// The existing housing broker firm ID.
        /// </summary>
        public int ExistingBrokerBrokerFirmId { get; }

        /// <summary>
        /// The existing housing broker ID.
        /// </summary>
        public int ExistingBrokerBrokerId { get; }
    }
}

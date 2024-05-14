using FribergFastigheter.Client.Services.AuthorizationHandlers.Housing;

namespace FribergFastigheter.Client.Services.AuthorizationHandlers.Housing.Data
{
    /// <summary>
    /// Interface designed to be used with the <see cref="ManageHousingPreAuthorizationHandler"/> to enable authorization for editing housing objects.
    /// </summary>
    /// <!-- Author: Jimmie -->
    /// <!-- Co Authors: -->
    public interface IEditHousingPreAuthorizationData
    {
        /// <summary>
        /// The existing housing broker firm ID.
        /// </summary>
        public int ExistingHousingBrokerFirmId { get; }

        /// <summary>
        /// The existing housing broker ID.
        /// </summary>
        public int ExistingHousingBrokerId { get; }
    }
}

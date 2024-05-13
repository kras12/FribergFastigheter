namespace FribergFastigheter.Shared.Services.AuthorizationHandlers.Data.Housing
{
    /// <summary>
    /// Interface designed to be used with the <see cref="ManageHousingAuthorizationHandler"/> to enable authorization for editing housing objects.
    /// </summary>
    /// <!-- Author: Jimmie -->
    /// <!-- Co Authors: -->
    public interface IEditHousingAuthorizationData
    {
        /// <summary>
        /// The existing housing broker firm ID.
        /// </summary>
        public int ExistingHousingBrokerFirmId { get; }

        /// <summary>
        /// The existing housing broker ID.
        /// </summary>
        public int ExistingHousingBrokerId { get; }

        /// <summary>
        /// The new housing broker ID.
        /// </summary>
        public int NewHousingBrokerId { get; }
    }
}

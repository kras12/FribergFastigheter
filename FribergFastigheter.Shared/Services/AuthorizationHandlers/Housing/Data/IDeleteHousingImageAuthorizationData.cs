namespace FribergFastigheter.Shared.Services.AuthorizationHandlers.Housing.Data
{
    /// <summary>
    /// Interface designed to be used with the <see cref="ManageHousingAuthorizationHandler"/> to enable authorization for deleting housing image objects.
    /// </summary>
    /// <!-- Author: Jimmie -->
    /// <!-- Co Authors: -->
    public interface IDeleteHousingImageAuthorizationData
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

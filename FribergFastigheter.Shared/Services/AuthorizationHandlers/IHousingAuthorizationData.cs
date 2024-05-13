namespace FribergFastigheter.Shared.Services.AuthorizationHandlers
{
    /// <summary>
    /// Interface designed to be used with the <see cref="ManageHousingAuthorizationHandler"/> to enable authorization.
    /// </summary>
    /// <!-- Author: Jimmie -->
    /// <!-- Co Authors: -->
    public interface IHousingAuthorizationData
    {
        /// <summary>
        /// The broker firm ID.
        /// </summary>
        public int? ExistingHousingBrokerFirmId { get; }

        /// <summary>
        /// The old broker ID.
        /// </summary>
        public int? ExistingHousingBrokerId { get; }

        /// <summary>
        /// The housing ID.
        /// </summary>
        public int? HousingId { get; }

        /// <summary>
        /// The new broker ID.
        /// </summary>
        public int? NewHousingBrokerId { get; }
    }
}

namespace FribergFastigheter.Client.Services.AuthorizationHandlers
{
    /// <summary>
    /// Interface designed to be used with the <see cref="ManageHousingPreAuthorizationHandler"/> to perform authorization.
    /// </summary>
    /// <!-- Author: Jimmie -->
    /// <!-- Co Authors: -->
    public interface IHousingPreAuthorizationData
    {
        /// <summary>
        /// The broker firm ID.
        /// </summary>
        public int ExistingHousingBrokerFirmId { get; }

        /// <summary>
        /// The broker ID.
        /// </summary>

        public int ExistingHousingBrokerId { get; }

        /// <summary>
        /// The housing ID.
        /// </summary>

        public int HousingId { get; }
    }
}

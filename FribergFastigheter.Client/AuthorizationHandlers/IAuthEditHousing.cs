namespace FribergFastigheter.Client.AuthorizationHandlers
{
    /// <summary>
    /// Interface designed to be used with the <see cref="ManageHousingAuthorizationHandler"/>.
    /// </summary>
    /// <!-- Author: Jimmie -->
    /// <!-- Co Authors: -->
    public interface IAuthEditHousing
    {
        /// <summary>
        /// The broker firm ID.
        /// </summary>
        public int BrokerFirmId { get; }

        /// <summary>
        /// The broker ID.
        /// </summary>

        public int BrokerId { get; }

        /// <summary>
        /// The housing ID.
        /// </summary>

        public int HousingId { get; }
    }
}

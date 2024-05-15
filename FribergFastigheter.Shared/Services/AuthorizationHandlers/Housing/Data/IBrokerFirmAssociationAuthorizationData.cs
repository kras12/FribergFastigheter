namespace FribergFastigheter.Shared.Services.AuthorizationHandlers.Housing.Data
{
    /// <summary>
    /// Interface designed to be used with the <see cref="ManageHousingAuthorizationHandler"/> to enable authorization to check whether a broker belongs to a broker firm.
    /// </summary>
    /// <!-- Author: Jimmie -->
    /// <!-- Co Authors: -->
    public interface IBrokerFirmAssociationAuthorizationData
    {
        /// <summary>
        /// The broker firm ID.
        /// </summary>
        public int BrokerFirmId { get; }
    }
}

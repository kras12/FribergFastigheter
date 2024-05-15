namespace FribergFastigheter.Shared.Services.AuthorizationHandlers.Housing.Data
{
    /// <summary>
    /// Data storage class designed to be used with the <see cref="ManageHousingAuthorizationHandler"/> to enable authorization to check whether a broker belongs to a broker firm.
    /// </summary>
    /// <!-- Author: Jimmie -->
    /// <!-- Co Authors: -->
    public class BrokerFirmAssociationAuthorizationData : IBrokerFirmAssociationAuthorizationData
    {
        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="brokerFirmId">The broker firm ID.</param>
        public BrokerFirmAssociationAuthorizationData(int brokerFirmId)
        {
            BrokerFirmId = brokerFirmId;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The broker firm ID.
        /// </summary>
        public int BrokerFirmId { get; }

        #endregion
    }
}

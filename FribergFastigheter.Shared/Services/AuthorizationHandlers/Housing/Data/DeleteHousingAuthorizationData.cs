namespace FribergFastigheter.Shared.Services.AuthorizationHandlers.Housing.Data
{
    /// <summary>
    /// Data storage class designed to be used with the <see cref="ManageHousingAuthorizationHandler"/> to enable authorization for deleting housing objects.
    /// </summary>
    /// <!-- Author: Jimmie -->
    /// <!-- Co Authors: -->
    public class DeleteHousingAuthorizationData : IDeleteHousingAuthorizationData
    {
        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="existingHousingBrokerFirmId">The existing housing broker firm ID.</param>
        /// <param name="existingHousingBrokerId">The existing housing broker ID.</param>
        public DeleteHousingAuthorizationData(int existingHousingBrokerFirmId, int existingHousingBrokerId)
        {
            ExistingHousingBrokerFirmId = existingHousingBrokerFirmId;
            ExistingHousingBrokerId = existingHousingBrokerId;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The existing housing broker firm ID.
        /// </summary>
        public int ExistingHousingBrokerFirmId { get; }

        /// <summary>
        /// The existing housing broker ID.
        /// </summary>
        public int ExistingHousingBrokerId { get; }

        #endregion
    }
}

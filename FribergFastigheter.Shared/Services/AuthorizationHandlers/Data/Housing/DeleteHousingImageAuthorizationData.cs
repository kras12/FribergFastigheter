namespace FribergFastigheter.Shared.Services.AuthorizationHandlers.Data.Housing
{
    /// <summary>
    /// Data storage class designed to be used with the <see cref="ManageHousingAuthorizationHandler"/> to enable authorization for deleting housing image objects.
    /// </summary>
    /// <!-- Author: Jimmie -->
    /// <!-- Co Authors: -->
    public class DeleteHousingImageAuthorizationData : IDeleteHousingImageAuthorizationData
    {
        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="existingHousingBrokerFirmId">The broker firm ID.</param>
        /// <param name="existingHousingBrokerId">The old broker ID.</param>
        public DeleteHousingImageAuthorizationData(int existingHousingBrokerFirmId,
            int existingHousingBrokerId)
        {
            ExistingHousingBrokerFirmId = existingHousingBrokerFirmId;
            ExistingHousingBrokerId = existingHousingBrokerId;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The broker firm ID.
        /// </summary>
        public int ExistingHousingBrokerFirmId { get; }

        /// <summary>
        /// The old broker ID.
        /// </summary>
        public int ExistingHousingBrokerId { get; }

        #endregion
    }
}

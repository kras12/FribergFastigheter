namespace FribergFastigheter.Shared.Services.AuthorizationHandlers.Data.Housing
{
    /// <summary>
    /// Data storage class designed to be used with the <see cref="ManageHousingAuthorizationHandler"/> to enable authorization for editing housing objects.
    /// </summary>
    /// <!-- Author: Jimmie -->
    /// <!-- Co Authors: -->
    public class EditHousingAuthorizationData : IEditHousingAuthorizationData
    {
        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="existingHousingBrokerFirmId">The broker firm ID.</param>
        /// <param name="existingHousingBrokerId">The old broker ID.</param>
        /// <param name="newHousingBrokerId">The new broker ID.</param>
        public EditHousingAuthorizationData(int existingHousingBrokerFirmId,
            int existingHousingBrokerId, int newHousingBrokerId)
        {
            NewHousingBrokerId = newHousingBrokerId;
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

        /// <summary>
        /// The new broker ID.
        /// </summary>
        public int NewHousingBrokerId { get; }

        #endregion
    }
}

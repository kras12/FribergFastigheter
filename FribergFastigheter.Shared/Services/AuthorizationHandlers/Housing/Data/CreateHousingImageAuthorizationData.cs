namespace FribergFastigheter.Shared.Services.AuthorizationHandlers.Housing.Data
{
    /// <summary>
    /// Data storage class designed to be used with the <see cref="ManageHousingAuthorizationHandler"/> to enable authorization for creating housing image objects.
    /// </summary>
    /// <!-- Author: Jimmie -->
    /// <!-- Co Authors: -->
    public class CreateHousingImageAuthorizationData : ICreateHousingImageAuthorizationData
    {
        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="existingHousingBrokerFirmId">The broker firm ID.</param>
        /// <param name="existingHousingBrokerId">The old broker ID.</param>
        public CreateHousingImageAuthorizationData(int existingHousingBrokerFirmId,
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

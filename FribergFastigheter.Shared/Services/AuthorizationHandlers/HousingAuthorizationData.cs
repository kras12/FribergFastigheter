namespace FribergFastigheter.Shared.Services.AuthorizationHandlers
{
    /// <summary>
    /// Data storage class designed to be used with the <see cref="ManageHousingAuthorizationHandler"/> to enable authorization.
    /// </summary>
    /// <!-- Author: Jimmie -->
    /// <!-- Co Authors: -->
    public class HousingAuthorizationData : IHousingAuthorizationData
    {
        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="housingId">The housing ID.</param>
        /// <param name="existingHousingBrokerFirmId">The broker firm ID.</param>
        /// <param name="existingHousingBrokerId">The old broker ID.</param>
        /// <param name="newHousingBrokerId">The new broker ID.</param>
        public HousingAuthorizationData(int? housingId = null, int? existingHousingBrokerFirmId = null,
            int? existingHousingBrokerId = null, int? newHousingBrokerId = null)
        {
            HousingId = housingId;
            NewHousingBrokerId = newHousingBrokerId;
            ExistingHousingBrokerFirmId = existingHousingBrokerFirmId;
            ExistingHousingBrokerId = existingHousingBrokerId;
        }

        #endregion

        #region Properties

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

        #endregion
    }
}

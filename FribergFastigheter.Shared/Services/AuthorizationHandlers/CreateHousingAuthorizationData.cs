namespace FribergFastigheter.Shared.Services.AuthorizationHandlers
{
    /// <summary>
    /// Data storage class designed to be used with the <see cref="ManageHousingAuthorizationHandler"/> to enable authorization for creating new housing objects.
    /// </summary>
    /// <!-- Author: Jimmie -->
    /// <!-- Co Authors: -->
    public class CreateHousingAuthorizationData : ICreateHousingAuthorizationData
    {
        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="newHousingBrokerId">The new broker ID.</param>
        public CreateHousingAuthorizationData(int? newHousingBrokerId = null)
        {
            NewHousingBrokerId = newHousingBrokerId;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The new broker ID.
        /// </summary>
        public int? NewHousingBrokerId { get; }

        #endregion
    }
}

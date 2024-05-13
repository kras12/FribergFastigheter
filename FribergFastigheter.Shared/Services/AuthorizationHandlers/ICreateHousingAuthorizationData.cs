namespace FribergFastigheter.Shared.Services.AuthorizationHandlers
{
    /// <summary>
    /// Interface designed to be used with the <see cref="ManageHousingAuthorizationHandler"/> to enable authorization for creating new housing objects.
    /// </summary>
    /// <!-- Author: Jimmie -->
    /// <!-- Co Authors: -->
    public interface ICreateHousingAuthorizationData
    {
        /// <summary>
        /// The new housing broker ID.
        /// </summary>
        public int? NewHousingBrokerId { get; }
    }
}

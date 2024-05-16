using FribergFastigheter.Shared.Dto.Housing;

namespace FribergFastigheter.Shared.Services.AuthorizationHandlers.Housing.Data
{
    /// <summary>
    /// Interface designed to be used with the <see cref="ManageHousingAuthorizationHandler"/> to enable authorization for editing housing objects.
    /// </summary>
    /// <!-- Author: Jimmie -->
    /// <!-- Co Authors: -->
    public interface IEditHousingAuthorizationData
    {
        /// <summary>
        /// The existing housing object.
        /// </summary>
        public HousingDto ExistingHousing { get; set; }

        /// <summary>
        /// The new housing object.
        /// </summary>
        public EditHousingDto NewHousing { get; set; }
    }
}

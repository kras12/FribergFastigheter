using FribergFastigheter.Shared.Dto.Housing;

namespace FribergFastigheter.Shared.Services.AuthorizationHandlers.Housing.Data
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
        /// <param name="existingHousing">The existing housing object.</param>
        /// <param name="newHousing">The new housing object.</param>
        public EditHousingAuthorizationData(HousingDto existingHousing, EditHousingDto newHousing)
        {
            ExistingHousing = existingHousing;
            NewHousing = newHousing;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The existing housing object.
        /// </summary>
        public HousingDto ExistingHousing { get; set; }

        /// <summary>
        /// The new housing object.
        /// </summary>
        public EditHousingDto NewHousing { get; set; }

        #endregion
    }
}

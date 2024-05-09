using System.ComponentModel.DataAnnotations;

namespace FribergFastigheter.Shared.Dto.Housing
{
    /// <summary>
    /// A DTO class that holds data for editing a housing.
    /// </summary>
    /// <!-- Author: Jimmie -->
    /// <!-- Co Authors: -->
    public class EditHousingDto : HousingBaseDto
    {
        #region Properties

        /// <summary>
        /// The ID of the broker associated with the housing object.
        /// </summary>
        public int BrokerId { get; set; }

        /// <summary>
        /// The ID of the category associated with the housing object.
        /// </summary>
        public int CategoryId { get; set; }

        /// <summary>
        /// The ID of the housing object.
        /// </summary>
        public int HousingId { get; set; }

        /// <summary>
        /// The ID of the municipality associated with the housing object.
        /// </summary>
        public int MunicipalityId { get; set; }

        #endregion
    }
}

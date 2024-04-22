using System.ComponentModel.DataAnnotations;

namespace FribergFastigheter.Shared.Dto
{
    /// <summary>
    /// A DTO class that represents a housing category.
    /// </summary>
    /// <!-- Author: Jimmie -->
    /// <!-- Co Authors: -->
    public class HousingCategoryDto
    {
        #region Properties

        /// <summary>
        /// The name of the category.
        /// </summary>
        public string CategoryName { get; set; } = string.Empty;

        /// <summary>
        /// The ID of the category.
        /// </summary>
        public int HousingCategoryId { get; set; }

        #endregion
    }
}

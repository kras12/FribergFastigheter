using System.ComponentModel.DataAnnotations;

namespace FribergFastigheter.Client.Models.Housing
{
    /// <summary>
    /// A view model class that represents a housing category.
    /// </summary>
    /// <!-- Author: Jimmie -->
    /// <!-- Co Authors: -->
    public class HousingCategoryViewModel : ViewModelBase
    {
        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        public HousingCategoryViewModel()
        {

        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="housingCategoryId">The ID of the category</param>
        /// <param name="categoryName">The name of the category.</param>
        public HousingCategoryViewModel(int housingCategoryId, string categoryName)
        {
            CategoryName = categoryName;
            HousingCategoryId = housingCategoryId;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The default category for including all categories.
        /// </summary>
        public static HousingCategoryViewModel AllCategories { get; } = new HousingCategoryViewModel(0, "Alla");

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

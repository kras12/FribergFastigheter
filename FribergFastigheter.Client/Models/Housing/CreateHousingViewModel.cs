using System.ComponentModel.DataAnnotations;

namespace FribergFastigheter.Client.Models.Housing
{
    /// <summary>
    /// A DTO class that holds data for housing creation.
    /// </summary>
    /// <!-- Author: Jimmie -->
    /// <!-- Co Authors: -->
    public class CreateHousingViewModel : HousingViewModelBase
    {
        #region Properties

        /// <summary>
        /// The ID of the broker associated with the housing object.
        /// </summary>
        public int BrokerId { get; set; }

        /// <summary>
        /// A collection of housing categories to choose from.
        /// </summary>
        public List<HousingCategoryViewModel> HousingCategories { get; set; } = new();

        /// <summary>
        /// A collection of municipalitites to choose from.
        /// </summary>
        public List<MunicipalityViewModel> Municipalities { get; set; } = new();

        /// <summary>
        /// The ID of the category selected for the new housing object.
        /// </summary>
        [Required]
        public int SelectedCategoryId { get; set; }

        /// <summary>
        /// The ID of the municipality elected for the new housing object.
        /// </summary>
        [Required]
        public int SelectedMunicipalityId { get; set; }

        #endregion
    }
}

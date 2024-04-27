using FribergFastigheter.Shared.Dto;

namespace FribergFastigheter.Client.Models
{
    /// <summary>
    /// View model for input data for a housing search form.
    /// </summary>
    /// <!-- Author: Jimmie -->
    /// <!-- Co Authors: -->
    public class HousingSearchInputViewModel
    {
        #region Properties

        /// <summary>
        /// A collection of housing categories.
        /// </summary>
        public List<HousingCategoryViewModel> HousingCategories { get; set; } = new();

        /// <summary>
        /// The maximum living area in m² of the housing object.
        /// </summary>
        public int MaxLivingArea { get; set; } = 300;

        /// <summary>
        /// The maximum price of the housing object.
        /// </summary>
        public int MaxPrice { get; set; } = 20_000_000;

        /// <summary>
        /// The minimum living area in m² of the housing object.
        /// </summary>
        public int MinLivingArea { get; set; } = 20;

        /// <summary>
        /// The minimum price of the housing object.
        /// </summary>
        public int MinPrice { get; set; } = 0;

        /// <summary>
        /// A collection of municipalities.
        /// </summary>
        public List<MunicipalityViewModel> Municipalities { get; set; } = new();

        /// <summary>
        /// The number of results to fetch per page.
        /// </summary>
        public int NumberOfResultsPerPage { get; set; } = 25;

        /// <summary>
        /// The selected housing category.
        /// </summary>
        public int SelectedCategoryId { get; set; }

        /// <summary>
        /// The selected municipality.
        /// </summary>
        public int SelectedMunicipalityId { get; set; }

        #endregion
    }
}

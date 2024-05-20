using FribergFastigheter.Client.Pages;
using System.ComponentModel.DataAnnotations;

namespace FribergFastigheter.Client.Models.Housing
{
    /// <summary>
    /// A view model base class that represents a housing object.
    /// </summary>
    /// <!-- Author: Jimmie -->
    /// <!-- Co Authors: Dan -->
    public class HousingViewModelBase : ViewModelBase
    {
        #region Properties

        /// <summary>
        /// The address of the housing object.
        /// </summary>
        [Required]
        [RegularExpression(LettersNumbersAndSpacesRegexPattern, ErrorMessage = OnlyLettersNumbersAndSpacesValidationMessage)]
        public string Address { get; set; } = "";

        /// <summary>
        /// The ancillaryArea (m2) of the housing object.
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = PositiveNumberValidationErrorMessage)]
        public double? AncillaryArea { get; set; }

        /// <summary>
        /// The build year of the housing object.
        /// </summary>
        [Range(1900, 2100, ErrorMessage = BuildYearValidationMessage)]
        public int? BuildYear { get; set; }

        /// <summary>
        /// The description of the housing object.
        /// </summary>
        [Required]
        [RegularExpression(BlackListDangerousCharactersExpression, ErrorMessage = BlackListDangerousCharactersValidationMessage)]
        public string Description { get; set; } = "";

        /// <summary>
        /// The land area (m2) of the housing object.
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = PositiveNumberValidationErrorMessage)]
        public double? LandArea { get; set; }

        /// <summary>
        /// The living area (m2) of the housing object.
        /// </summary>
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = PositiveNumberValidationErrorMessage)]
        public double LivingArea { get; set; }

        /// <summary>
        /// The monthly fee of the housing object.
        /// </summary>
        [Range(1, double.MaxValue, ErrorMessage = PositiveNumberValidationErrorMessage)]
        public decimal? MonthlyFee { get; set; } = null;

        /// <summary>
        /// The price of the housing object.
        /// </summary>
        [Required]
        [Range(1, double.MaxValue, ErrorMessage = PositiveNumberValidationErrorMessage)]
        public decimal Price { get; set; }

        /// <summary>
        /// The number of rooms of the housing object.
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = PositiveNumberValidationErrorMessage)]
        public int? RoomCount { get; set; } = null;

        /// <summary>
        /// The yearly running cost of the housing object.
        /// </summary>
        [Range(1, double.MaxValue, ErrorMessage = PositiveNumberValidationErrorMessage)]
        public decimal? YearlyRunningCost { get; set; } = null;

        #endregion
    }
}

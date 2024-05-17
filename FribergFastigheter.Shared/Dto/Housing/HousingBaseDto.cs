using System.ComponentModel.DataAnnotations;

namespace FribergFastigheter.Shared.Dto.Housing
{
    /// <summary>
    /// A base DTO class that represents a housing object.
    /// </summary>
    /// <!-- Author: Jimmie -->
    /// <!-- Co Authors: -->
    public class HousingBaseDto
    {
        #region ValidationExpressions

        /// <summary>
        /// Regular expression pattern to only allow letters, numbers and spaces. 
        /// </summary>
        protected const string LettersNumbersAndSpacesRegexPattern = @"^[\p{L}\p{N} ]+$";

        #endregion

        #region ValidationErrorMessages

        /// <summary>
        /// Validation error message for build year.
        /// </summary>
        public const string BuildYearValidationMessage = "Byggåret måste vara mellan 1900 och 2100";

        /// <summary>
        /// A message to inform the user that only letters, numbers and spaces are allowed as input. 
        /// </summary>
        protected const string OnlyLettersNumbersAndSpacesValidationMessage = "Enbart bokstäver, number och mellanslag är tillåtet.";

        /// <summary>
        /// Validation error message for positive number validation.
        /// </summary>
        public const string PositiveNumberValidationErrorMessage = "Ange ett positivt tal större än noll.";

        #endregion        

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
        public string Description { get; set; } = "";

        /// <summary>
        /// The land area (m2) of the housing object.
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = PositiveNumberValidationErrorMessage)]
        public double? LandArea { get; set; }

        /// <summary>
        /// The living area (m2) of the housing object.
        /// </summary>
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

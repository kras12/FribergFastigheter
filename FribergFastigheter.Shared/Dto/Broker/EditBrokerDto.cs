using System.ComponentModel.DataAnnotations;

namespace FribergFastigheter.Shared.Dto.Broker
{
    /// <summary>
    /// A DTO class that contains data for when a broker is editing themselves.
    /// </summary>
    /// <!-- Author: Marcus -->
    /// <!-- Co Authors: -->
    public class EditBrokerDto
    {
        #region ValidationExpressions

        /// <summary>
        /// Regular expression for email validation.
        /// </summary>
        public const string EmailValidationExpression = @"^[\p{L}\p{N}\._\-]+\@[\p{L}\p{N}\.\-]+\.\p{L}+$", ErrorMessage = "Ogiltig epostadress.";

        /// <summary>
        /// Regular expression for name validation.
        /// </summary>
        public const string NameValidationExpression = @"^[\p{L} ]+$";

        /// <summary>
        /// Regular expression for password validation.
        /// </summary>
        public const string PasswordValididationExpression = @"^(?=.*?\p{Ll})(?=.*?\p{Lu})(?=.*?[0-9])(?=.*?[#?!@$ %^&*-]).{10,}$";

        #endregion

        #region ValidationErrorMessages

        /// <summary>
        /// Validation error message for email validation.
        /// </summary>
        public const string EmailValidationErrorMessage = "Ogiltig epostadress.";

        /// <summary>
        /// Validation error message for name validation.
        /// </summary>
        public const string NameValidationErrorMessage = "Enbart bokstäver och mellanslag är tillåtet.";

        /// <summary>
        /// Validation error message for password validation.
        /// </summary>
        public const string PasswordValidationErrorMessage = "Lösenordet måste vara minst 10 tecken och innehålla stora och små bokstäver, siffror, och specialtecken (#?!@$ %^&*-)";

        #endregion

        #region Properties

        /// <summary>
        /// The ID of the broker.
        /// </summary>
        [Required]
        public int BrokerId { get; set; }

        /// <summary>
        /// The description of the broker.
        /// </summary>
        public string Description { get; set; } = "";

        /// <summary>
        /// The email of the broker.
        /// </summary>
        [Required]
        [EmailAddress]
        [RegularExpression(EmailValidationExpression, ErrorMessage = EmailValidationErrorMessage)]
        public string Email { get; set; } = "";

        /// <summary>
        /// The first name of the broker.
        /// </summary>
        [Required]
        [RegularExpression(NameValidationExpression, ErrorMessage = NameValidationErrorMessage)]
        public string FirstName { get; set; } = "";

        /// <summary>
        /// The last name of the broker.
        /// </summary>
        [Required]
        [RegularExpression(NameValidationExpression, ErrorMessage = NameValidationErrorMessage)]
        public string LastName { get; set; } = "";

        /// <summary>
        /// The phone number of the broker.
        /// </summary>
        [Required]
        [Phone]
        public string PhoneNumber { get; set; } = "";

        #endregion
    }
}


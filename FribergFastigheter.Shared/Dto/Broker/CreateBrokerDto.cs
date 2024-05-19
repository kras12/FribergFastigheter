using System.ComponentModel.DataAnnotations;

namespace FribergFastigheter.Shared.Dto.Broker
{
    /// <summary>
    /// A DTO class that holds data for creating a broker. 
    /// </summary>
    /// <!-- Author: Marcus -->
    /// <!-- Co Authors: -->
    public class CreateBrokerDto
    {
        #region ValidationExpressions

        /// <summary>
        /// Regular expression for blacklisting of dangerous characters.
        /// </summary>
        public const string BlackListDangerousCharactersExpression = @"^[^<>]+$";

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
        /// Validation error message for blacklisting of dangerous characters
        /// </summary>
        public const string BlackListDangerousCharactersValidationMessage = "Följande tecken är inte tillåtna: '<>'";

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
        /// The description of the broker.
        /// </summary>
        [RegularExpression(BlackListDangerousCharactersExpression, ErrorMessage = BlackListDangerousCharactersValidationMessage)]
        public string Description { get; set; } = "";

        /// <summary>
        /// The username/email of the broker.
        /// </summary>
        [Required]
        [EmailAddress(ErrorMessage = EmailValidationErrorMessage)]
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
        ///The password for the broker.
        /// </summary>
        [Required]
        [RegularExpression(PasswordValididationExpression, ErrorMessage = PasswordValidationErrorMessage)]
        public string Password { get; set; } = "";

        /// <summary>
        /// The phone number of the broker.
        /// </summary>
        [Required]
        [Phone]
        public string PhoneNumber { get; set; } = "";

        #endregion
    }
}

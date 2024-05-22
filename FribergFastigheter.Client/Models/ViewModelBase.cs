using System.Security.Policy;

namespace FribergFastigheter.Client.Models
{
    /// <summary>
    /// ViewModel base class.
    /// </summary>
    /// <!-- Author: Jimmie -->
    /// <!-- Co Authors: -->
    public abstract class ViewModelBase
    {
        #region ValidationExpressions

        /// <summary>
        /// Regular expression for email validation.
        /// </summary>
        public const string EmailValidationExpression = @"^[\p{L}\p{N}\._\-]+\@[\p{L}\p{N}\.\-]+\.\p{L}+$", ErrorMessage = "Ogiltig epostadress.";

        /// <summary>
        /// Regular expression for blacklisting of dangerous characters.
        /// </summary>
        public const string BlackListDangerousCharactersExpression = @"^[^<>]+$";

        /// <summary>
        /// Regular expression pattern to only allow letters, numbers and spaces. 
        /// </summary>
        protected const string LettersAndSpacesRegexPattern = @"^[\p{L} ]+$";

        /// <summary>
        /// Regular expression pattern to only allow letters, numbers and spaces. 
        /// </summary>
        protected const string LettersNumbersAndSpacesRegexPattern = @"^[\p{L}\p{N} ]+$";

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
        /// Validation error message for build year.
        /// </summary>
        public const string BuildYearValidationMessage = "Byggåret måste vara mellan 1900 och 2100";

        /// <summary>
        /// Validation error message for email validation.
        /// </summary>
        public const string EmailValidationErrorMessage = "Ogiltig epostadress.";

        /// <summary>
        /// Validation error message for name validation.
        /// </summary>
        public const string NameValidationErrorMessage = "Enbart bokstäver och mellanslag är tillåtet.";

        /// <summary>
        /// A message to inform the user that only letters, numbers and spaces are allowed as input. 
        /// </summary>
        protected const string OnlyLettersNumbersAndSpacesValidationMessage = "Enbart bokstäver, number och mellanslag är tillåtet.";

        /// <summary>
        /// Validation error message for password validation.
        /// </summary>
        public const string PasswordValidationErrorMessage = "Lösenordet måste vara minst 10 tecken och innehålla stora och små bokstäver, siffror, och specialtecken (#?!@$ %^&*-)";

        /// <summary>
        /// Validation error message for positive number validation.
        /// </summary>
        public const string PositiveNumberValidationErrorMessage = "Ange ett positivt tal större än noll.";

        #endregion
    }
}

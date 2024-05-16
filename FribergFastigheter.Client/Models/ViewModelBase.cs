using System.Security.Policy;

namespace FribergFastigheter.Client.Models
{
    /// <summary>
    /// ViewModel base class.
    /// </summary>
    public abstract class ViewModelBase
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

        public const string BuildYearValidationMessage = "Byggåret måste vara mellan 1900 och 2100";

        /// <summary>
        /// Validation error message for email validation.
        /// </summary>
        public const string EmailValidationErrorMessage = "Ogiltig epostadress.";

        /// <summary>
        /// Validation error message for name validation.
        /// </summary>
        public const string NameValidationErrorMessage = "Får endast innehålla bokstäver och mellanrum.";

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

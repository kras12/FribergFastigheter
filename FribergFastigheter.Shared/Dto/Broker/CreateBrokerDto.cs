using System.ComponentModel.DataAnnotations;

namespace FribergFastigheter.Shared.Dto.Broker
{
    /// <summary>
    /// A DTO class that holds data for creating a broker. 
    /// </summary>
    /// <!-- Author: Marcus, Marcus -->
    /// <!-- Co Authors: -->
    public class CreateBrokerDto : DtoValidationBase
    {
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

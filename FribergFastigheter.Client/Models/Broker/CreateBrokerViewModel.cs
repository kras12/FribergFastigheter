using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FribergFastigheter.Client.Models.Broker
{
    /// <summary>
    /// A view model class that represents a broker.
    /// </summary>
    /// <!-- Author: Jimmie -->
    /// <!-- Co Authors: -->
    public class CreateBrokerViewModel : ViewModelBase
    {
        #region Properties

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

        /// <summary>
        /// The password for the broker.
        /// </summary>
        [Required]
        [RegularExpression(PasswordValididationExpression, ErrorMessage = PasswordValidationErrorMessage)]
        public string Password { get; set; } = "";

        #endregion
    }
}

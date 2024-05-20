using System.ComponentModel.DataAnnotations;

namespace FribergFastigheter.Shared.Dto.Broker
{
    /// <summary>
    /// A DTO class that contains data for when a broker is editing themselves.
    /// </summary>
    /// <!-- Author: Marcus -->
    /// <!-- Co Authors: -->
    public class EditBrokerDto : DtoValidationBase
    {
        #region Properties

        /// <summary>
        /// The ID of the broker.
        /// </summary>
        [Required]
        public int BrokerId { get; set; }

        /// <summary>
        /// The description of the broker.
        /// </summary>
        [RegularExpression(BlackListDangerousCharactersExpression, ErrorMessage = BlackListDangerousCharactersValidationMessage)]
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


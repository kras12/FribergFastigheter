using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FribergFastigheter.Client.Models.Broker
{
    /// <summary>
    /// A view model class that represents a broker.
    /// </summary>
    /// <!-- Author: Jimmie -->
    /// <!-- Co Authors: -->
    public class CreateBrokerViewModel
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
        [RegularExpression(@"^[\p{L}\p{N}\._\-]+\@[\p{L}\p{N}\.\-]+\.\p{L}+$", ErrorMessage = "Ogiltig epostadress.")]
        public string Email { get; set; } = "";

        /// <summary>
        /// The first name of the broker.
        /// </summary>
        [Required]
        [RegularExpression(@"^[\p{L} ]+$", ErrorMessage = "Får endast innehålla bokstäver och mellanrum.")]
        public string FirstName { get; set; } = "";

        /// <summary>
        /// The last name of the broker.
        /// </summary>
        [Required]
        [RegularExpression(@"^[\p{L} ]+$", ErrorMessage = "Får endast innehålla bokstäver och mellanrum.")]
        public string LastName { get; set; } = "";

        /// <summary>
        /// The phone number of the broker.
        /// </summary>
        public string PhoneNumber { get; set; } = "";

        /// <summary>
        /// The password for the broker.
        /// </summary>
        [Required]
        [RegularExpression(@"^(?=.*?\p{Ll})(?=.*?\p{Lu})(?=.*?[0-9])(?=.*?[#?!@$ %^&*-]).{10,}$", ErrorMessage = "Lösenordet måste vara minst 10 tecken och innehålla stora och små bokstäver, siffror, och specialtecken (#?!@$ %^&*-)")]
        public string Password { get; set; } = "";

        #endregion
    }
}

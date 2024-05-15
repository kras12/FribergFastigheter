using System.ComponentModel.DataAnnotations;

namespace FribergFastigheter.Client.Models.BrokerFirm
{
    /// <summary>
    /// A view model that stores data for a login form. 
    /// </summary>
    /// <!-- Author: Jimmie -->
    /// <!-- Co Authors: -->
    public class LoginViewModel : ViewModelBase
    {
        #region Properties

        /// <summary>
        /// The user password. 
        /// </summary>
        [Required]
        [RegularExpression(PasswordValididationExpression, ErrorMessage = PasswordValidationErrorMessage)]
        public string Password { get; set; } = "";

        /// <summary>
        /// The user name. 
        /// </summary>
        [Required]
        public string UserName { get; set; } = "";

        #endregion
    }
}

using System.ComponentModel.DataAnnotations;

namespace FribergFastigheter.Client.Models.BrokerFirm
{
    /// <summary>
    /// A view model that stores data for a login form. 
    /// </summary>
    /// <!-- Author: Jimmie -->
    /// <!-- Co Authors: -->
    public class LoginViewModel
    {
        #region Properties

        /// <summary>
        /// The user password. 
        /// </summary>
        [Required]
        [RegularExpression(@"^(?=.*?\p{Ll})(?=.*?\p{Lu})(?=.*?[0-9])(?=.*?[#?!@$ %^&*-]).{10,}$", ErrorMessage = "Lösenordet måste vara minst 10 tecken och innehålla stora och små bokstäver, siffror, och specialtecken (#?!@$ %^&*-)")]
        public string Password { get; set; } = "";

        /// <summary>
        /// The user name. 
        /// </summary>
        [Required]
        public string UserName { get; set; } = "";

        #endregion
    }
}

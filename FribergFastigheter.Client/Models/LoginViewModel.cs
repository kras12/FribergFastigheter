namespace FribergFastigheter.Client.Models
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
        public string Password { get; set; } = "";

        /// <summary>
        /// The user name. 
        /// </summary>
        public string UserName { get; set; } = "";

        #endregion
    }
}

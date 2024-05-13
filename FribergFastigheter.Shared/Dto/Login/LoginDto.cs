using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FribergFastigheter.Shared.Dto.Login
{
    /// <summary>
    /// A DTO class for logging in a user.
    /// </summary>
    /// <!-- Author: Marcus, Jimmie -->
    /// <!-- Co Authors: -->
    public class LoginDto
    {
        #region Properties

        /// <summary>
        /// The username.
        /// </summary>
        [Required]
        public string UserName { get; set; } = "";

        /// <summary>
        /// The password.
        /// </summary>
        [Required]
        public string Password { get; set; } = "";

        #endregion
    }
}

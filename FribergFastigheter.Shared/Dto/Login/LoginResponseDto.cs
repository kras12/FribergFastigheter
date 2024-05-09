using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FribergFastigheter.Shared.Dto.Login
{
    /// <summary>
    /// A DTO class containing data for a login response.
    /// </summary>
    /// <!-- Author: Marcus, Jimmie -->
    /// <!-- Co Authors: -->
    public class LoginResponseDto
    {
        #region Properties

        /// <summary>
        /// The username.
        /// </summary>
        [Required]
        public string UserName { get; set; } = "";

        /// <summary>
        /// The token.
        /// </summary>
        [Required]
        public string Token { get; set; } = "";

        #endregion
    }
}

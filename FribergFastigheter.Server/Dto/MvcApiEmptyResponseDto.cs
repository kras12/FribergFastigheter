using FribergFastigheter.Shared.Dto.Api;
using FribergFastigheter.Shared.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FribergFastigheter.Server.Dto
{
    /// <summary>
    /// API empty response class for Friberg Fastigheter APIs.
    /// </summary>
    /// <!-- Author: Jimmie -->
    /// <!-- Co Authors: -->
    public class MvcApiEmptyResponseDto : ApiResponseDto<object>
    {
        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public MvcApiEmptyResponseDto()
        {
            
        }

        #endregion
    }
}

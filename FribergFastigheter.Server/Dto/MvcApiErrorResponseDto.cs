using FribergFastigheter.Shared.Dto.Api;
using FribergFastigheter.Shared.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace FribergFastigheter.Server.Dto
{
    /// <summary>
    /// API error response class for Friberg Fastigheter APIs that supports MVC related error sources from identity.
    /// </summary>
    /// <!-- Author: Jimmie -->
    /// <!-- Co Authors: -->
    public class MvcApiErrorResponseDto : ApiResponseDto<object>
    {
        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="errors">A collection of errors.</param>
        public MvcApiErrorResponseDto(List<KeyValuePair<string, string>> errors) : base(errors)
        {
  
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="errorType">The type of error.</param>
        /// <param name="errorMessage">The error message.</param>
        public MvcApiErrorResponseDto(ApiErrorMessageTypes errorType, string errorMessage)
            : base(errorType.ToString(), errorMessage)
        {

        }

        /// <summary>
        /// Constructor. 
        /// </summary>
        /// <param name="authorizationResult">The authorization result containing errors.</param>
        public MvcApiErrorResponseDto(AuthorizationResult authorizationResult)
        {
            Errors = authorizationResult.Failure!.FailureReasons
                    .Select(x => new KeyValuePair<string, string>(ApiErrorMessageTypes.AuthorizationError.ToString(), x.Message)).ToList();
            Success = false;
        }

        /// <summary>
        /// Constructor. 
        /// </summary>
        /// <param name="identityResult">The identity result containing errors.</param>
        public MvcApiErrorResponseDto(IdentityResult identityResult)
        {
            Errors = identityResult.Errors.Select(x => new KeyValuePair<string, string>(x.Code, x.Description)).ToList();
            Success = false;
        }

        #endregion
    }
}

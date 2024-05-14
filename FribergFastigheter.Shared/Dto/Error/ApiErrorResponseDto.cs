using FribergFastigheter.Shared.Enums;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Net;

namespace FribergFastigheter.Shared.Dto.Error
{
    /// <summary>
    /// A DTO class that represents an error response.
    /// </summary>
    /// <!-- Author: Jimmie -->
    /// <!-- Co Authors: -->
    public class ApiErrorResponseDto
    {
        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="statusCode">The status code.</param>
        /// <param name="error">The error.</param>
        /// <param name="errors">A collection of errors.</param>
        public ApiErrorResponseDto(HttpStatusCode statusCode, ApiErrorDto? error = null, List<ApiErrorDto>? errors = null)
        {
            StatusCode = (int)statusCode;
            StatusCodeDescription = statusCode.ToString();

            if (error != null)
            {
                Errors.Add(error);
            }

            if (errors != null)
            {
                Errors.AddRange(errors);
            }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="statusCode">The status code.</param>
        /// <param name="authorizationResult">The result of an authorization attempt.</param>
        public ApiErrorResponseDto(HttpStatusCode statusCode, AuthorizationResult authorizationResult)
        {
            StatusCode = (int)statusCode;
            StatusCodeDescription = statusCode.ToString();

            if (authorizationResult.Failure != null)
            {
                Errors.Add(new ApiErrorDto(ApiErrorMessageTypes.AuthorizationError,
                    errorMessages: authorizationResult.Failure.FailureReasons.Select(x => Enum.Parse<HousingAuthorizationFailureReasons>(x.Message).ToString()).ToList()));
            }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="statusCode">The status code.</param>
        /// <param name="identityResult">The identity result.</param>
        public ApiErrorResponseDto(HttpStatusCode statusCode, IdentityResult identityResult)
        {
            StatusCode = (int)statusCode;
            StatusCodeDescription = statusCode.ToString();
            Errors.Add(new ApiErrorDto(ApiErrorMessageTypes.IdentityError, errorMessages: identityResult.Errors.ToList()));
        }

        #endregion

        #region Properties

        /// <summary>
        /// A collection of errors. 
        /// </summary>
        public List<ApiErrorDto> Errors { get; set; } = new();

        /// <summary>
        /// The status code.
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// The status code description.
        /// </summary>
        public string StatusCodeDescription { get; set; }

        #endregion
    }
}

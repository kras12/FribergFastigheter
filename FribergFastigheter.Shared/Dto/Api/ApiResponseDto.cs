using FribergFastigheter.Shared.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FribergFastigheter.Shared.Dto.Api
{
    /// <summary>
    /// API response class for Friberg Fastigheter APIs.
    /// </summary>
    /// <!-- Author: Jimmie -->
    /// <!-- Co Authors: -->
    public class ApiResponseDto<T> where T : class
    {
        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public ApiResponseDto()
        {
            
        }

        /// <summary>
        /// Constructor for a successful response.
        /// </summary>
        /// <param name="value">The value to send in the response body.</param>
        public ApiResponseDto(T? value = null)
        {
            Value = value;
            Success = true;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="errors">A collection of errors.</param>
        public ApiResponseDto(List<KeyValuePair<string, string>> errors)
        {
            Errors = errors;
            Success = false;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="errorType">The type of error.</param>
        /// <param name="errorMessage">The error message.</param>
        public ApiResponseDto(string errorType, string errorMessage)
            : this(new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>(errorType, errorMessage) })
        {

        }

        #endregion

        #region Properties

        /// <summary>
        /// A collection of errors. 
        /// </summary>
        public List<KeyValuePair<string, string>> Errors { get; set; } = new();

        /// <summary>
        /// True if operation was successful.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// The value for a successful response.
        /// </summary>
        public T? Value { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Formats the error collection as a string.
        /// </summary>
        /// <returns>A <see cref="string"/>.</returns>
        public string GetErrorsAsString()
        {
            var stringBuilder = new StringBuilder();
            Errors.ForEach(x => stringBuilder.AppendLine($"{x.Key}: {x.Value}"));
            return stringBuilder.ToString();
        }

        #endregion
    }
}

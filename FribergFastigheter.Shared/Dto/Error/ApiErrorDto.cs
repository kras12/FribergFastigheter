using FribergFastigheter.Shared.Enums;

namespace FribergFastigheter.Shared.Dto.Error
{
    /// <summary>
    /// An error class designed to be used with the class <see cref="ApiErrorResponseDto"/>.
    /// </summary>
    /// <!-- Author: Jimmie -->
    /// <!-- Co Authors: -->
    public class ApiErrorDto
    {
        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="errorType">The type of error.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <param name="errorMessages">A collection of error messages.</param>
        public ApiErrorDto(string errorType, string? errorMessage = null, List<string>? errorMessages = null)
        {
            ErrorType = errorType;

            if (errorMessage != null)
            {
                ErrorMessages.Add(errorMessage);
            }

            if (errorMessages != null)
            {
                ErrorMessages.AddRange(errorMessages);
            }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="errorType">The type of error.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <param name="errorMessages">A collection of error messages.</param>
        public ApiErrorDto(ApiErrorMessageTypes errorType, string? errorMessage = null, List<string>? errorMessages = null)
        {
            ErrorType = errorType.ToString();

            if (errorMessage != null)
            {
                ErrorMessages.Add(errorMessage);
            }

            if (errorMessages != null)
            {
                ErrorMessages.AddRange(errorMessages);
            }
        }

        #endregion

        #region Properties

        // A collection of error descriptions. 
        public List<string> ErrorMessages { get; set; } = new();

        /// <summary>
        /// The type of error. 
        /// </summary>
        public string ErrorType { get; set; } = "";

        #endregion
    }
}

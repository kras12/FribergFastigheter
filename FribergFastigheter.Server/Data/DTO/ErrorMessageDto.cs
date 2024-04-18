using System.Net;

namespace FribergFastigheter.Server.Data.DTO
{
	// <summary>
	/// A DTO class that represents an error message.
	/// </summary>
	/// <!-- Author: Jimmie -->
	/// <!-- Co Authors: -->
	public class ErrorMessageDto
	{

		#region Constructors

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="statusCode">The status code.</param>
		/// <param name="errorMessage">The error message.</param>
		public ErrorMessageDto(HttpStatusCode statusCode, string errorMessage)
		{
			ErrorMessage = errorMessage;
			StatusCode = (int)statusCode;
			StatusCodeDescription = statusCode.ToString();
		}

		#endregion

		#region Properties

		/// <summary>
		/// The error message.
		/// </summary>
		public string ErrorMessage { get; set; } = "";


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

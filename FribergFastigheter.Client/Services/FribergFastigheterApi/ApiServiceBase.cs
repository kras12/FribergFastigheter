using System.Text;

namespace FribergFastigheter.Client.Services.FribergFastigheterApi
{
    /// <summary>
    /// Base class for all Friberg Fastigheter API services.
    /// </summary>
    /// <!-- Author: Jimmie -->
    /// <!-- Co Authors: -->
    public class ApiServiceBase
    {
        #region Fields

        /// <summary>
        /// The injected HTTP client.
        /// </summary>
        protected readonly HttpClient _httpClient;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="httpClient">The injected HTTP client.</param>
        protected ApiServiceBase(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Builds the query string to send with a request. 
        /// </summary>
        /// <param name="queries">A collection of key value pairs for the queries to include.</param>
        /// <returns>A formatted query <see cref="string"/>, or an empty string if the collection is empty.</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        protected string BuildQueryString(List<KeyValuePair<string, string>> queries)
        {
            #region Checks


            if (queries.Any(x => string.IsNullOrEmpty(x.Key) || string.IsNullOrEmpty(x.Value)))
            {
                throw new ArgumentException("The query collection contains invalid parameters.", nameof(queries));
            }

            #endregion

            StringBuilder stringBuilder = new();

            foreach (var query in queries)
            {
                if (stringBuilder.Length == 0)
                {
                    stringBuilder.Append("?");
                }
                else
                {
                    stringBuilder.Append("&");
                }

                stringBuilder.Append($"{query.Key}={query.Value}");
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Builds the query string to send with a request.
        /// </summary>
        /// <param name="key">The name of the parameter.</param>
        /// <param name="value">The value of the parameter.</param>
        /// <returns>A <see cref="string"/>.</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        protected string BuildQueryString(string key, string value)
        {
            return BuildQueryString(new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>(key, value) });
        }

        /// <summary>
        /// Checks the value of an object reference and throws an exception if it's null. 
        /// If it has a value the object is returned. 
        /// </summary>
        /// <param name="targetObject">The object to check.</param>
        /// <param name="exceptionMessage">An optional message to use for the exception.</param>
        /// <returns>The object if it's not null.</returns>
        /// <exception cref="Exception"></exception>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        protected T EnsureNotNull<T>(T? targetObject, string? exceptionMessage = null) where T : class
        {
            if (targetObject == null)
            {
                throw new Exception(exceptionMessage);
            }

            return targetObject;
        }

        #endregion
    }
}

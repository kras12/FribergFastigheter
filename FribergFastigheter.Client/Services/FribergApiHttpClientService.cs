namespace FribergFastigheter.Client.Services
{
    /// <summary>
    /// Wrapper class that provides a preconfigured http client for the Friberg Fastigheter API endpoints.
    /// </summary>
    /// <!-- Author: Jimmie -->
    /// <!-- Co Authors: -->
    public class FribergApiHttpClientService
    {
        #region Constructors

        /// <summary>
        /// The injected http client.
        /// </summary>
        /// <param name="httpClient"></param>
        public FribergApiHttpClientService(HttpClient httpClient)
        {
            HttpClient = httpClient;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The injected http client.
        /// </summary>
        public HttpClient HttpClient { get; private set; }

        #endregion
    }
}

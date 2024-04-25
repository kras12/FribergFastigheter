using FribergFastigheter.Shared.Dto;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;

namespace FribergFastigheter.Client.Services.HousingApi
{
    /// <summary>
    /// A service to fetch data from the Friberg Fastigheter Housing Broker API endpoint.
    /// </summary>
    /// <!-- Author: Jimmie -->
    /// <!-- Co Authors: -->
    public class HousingBrokerApiService : FribergApiServiceBase, IHousingBrokerApiService
    {
        #region Constants

        private const string ApiEndPoint = "api/Housing/Broker";

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="httpClient">The injected HTTP client service.</param>
        public HousingBrokerApiService(FribergApiHttpClientService httpClientService) : base(httpClientService)
        {

        }

        #endregion

        #region Methods

        /// <summary>
        /// Fetches data for a broker. 
        /// </summary>
        /// <param name="id">The ID of the broker.</param>
        /// <returns>A <see cref="Task"/> containing a collection of <see cref="BrokerDto"/> objects.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        public async Task<BrokerDto?> GetBrokerById(int id)
        {
            return await _httpClient.GetFromJsonAsync<BrokerDto>($"{ApiEndPoint}/{id}");
        }

        #endregion
    }
}

using FribergFastigheter.Shared.Dto;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;

namespace FribergFastigheter.Client.Services.HousingApi
{
    /// <summary>
    /// A service to fetch data from the Friberg Fastigheter Housing Broker Firm API endpoint.
    /// </summary>
    /// <!-- Author: Jimmie -->
    /// <!-- Co Authors: -->
    public class HousingBrokerFirmApiService : FribergApiServiceBase, IHousingBrokerFirmApiService
    {
        #region Constants

        private const string ApiEndPoint = "api/Housing/BrokerFirm";

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="httpClientService">The injected HTTP client service.</param>
        public HousingBrokerFirmApiService(FribergApiHttpClientService httpClientService) : base(httpClientService)
        {

        }

        #endregion

        #region Methods

        /// <summary>
        /// Fetches data for a broker firm. 
        /// </summary>
        /// <param name="id">The ID of the broker firm.</param>
        /// <returns>A <see cref="Task"/> containing a <see cref="BrokerFirmDto"/> object.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        public async Task<BrokerFirmDto?> GetBrokerFirmById(int id)
        {
            return await _httpClient.GetFromJsonAsync<BrokerFirmDto>($"{ApiEndPoint}/{id}");
        }

        #endregion
    }
}

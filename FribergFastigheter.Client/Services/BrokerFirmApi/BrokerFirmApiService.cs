using FribergFastigheter.Shared.Dto;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;

namespace FribergFastigheter.Client.Services.HousingApi
{
    /// <summary>
    /// A service to fetch data from the Friberg Fastigheter Broker Firm API endpoint.
    /// </summary>
    /// <!-- Author: Jimmie -->
    /// <!-- Co Authors: -->
    public class BrokerFirmApiService : FribergApiServiceBase, IBrokerFirmApiService
    {
        #region Constants

        private const string ApiEndPoint = "api/BrokerFirm";

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="httpClientService">The injected HTTP client service.</param>
        public BrokerFirmApiService(FribergApiHttpClientService httpClientService) : base(httpClientService)
        {

        }

        #endregion

        #region OpenAPiMethods

        /// <summary>
        /// Fetches a broker firm.
        /// </summary>
        /// <param name="brokerFirmId">The ID of the brokerfirm.</param>
        /// <returns>A <see cref="Task"/> containing a <see cref="BrokerFirmDto"/> object.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        public Task<BrokerFirmDto?> GetBrokerFirmById([Required] int brokerFirmId)
        {
            return _httpClient.GetFromJsonAsync<BrokerFirmDto>($"{ApiEndPoint}/{brokerFirmId}");
        }

        #endregion
    }
}

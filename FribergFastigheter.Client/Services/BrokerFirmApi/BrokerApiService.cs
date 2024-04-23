using FribergFastigheter.Shared.Dto;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;

namespace FribergFastigheter.Client.Services.HousingApi
{
    /// <summary>
    /// A service to fetch data from the Friberg Fastigheter Broker API endpoint.
    /// </summary>
    /// <!-- Author: Jimmie -->
    /// <!-- Co Authors: -->
    public class BrokerApiService : FribergApiServiceBase, IBrokerApiService
    {
        #region Constants

        private const string ApiEndPoint = "api/BrokerFirm/Broker";

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="httpClientService">The injected HTTP client service.</param>
        public BrokerApiService(FribergApiHttpClientService httpClientService) : base(httpClientService)
        {

        }

        #endregion

        #region OpenAPiMethods

        /// <summary>
        /// Creates a new broker under the broker firm.
        /// </summary>
        /// <param name="brokerFirmId">The ID of the brokerfirm that the broker belongs to.</param>
        /// <param name="broker">The serialized DTO object to send.</param>
        /// <returns>A <see cref="Task"/>.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        public Task CreateBroker([Required] int brokerFirmId, [Required] CreateBrokerDto broker)
        {
            return _httpClient.PostAsJsonAsync($"{ApiEndPoint}/{BuildQueryString("brokerFirmId", brokerFirmId.ToString())}", broker);
        }

        /// <summary>
        /// Deletes a broker.
        /// </summary>
        /// <param name="id">The ID of the broker object to delete.</param>
		/// <param name="brokerFirmId">The ID of the broker firm associated with the housing.</param>
        /// <returns>A <see cref="Task"/>.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        public Task DeleteBroker(int id, [Required] int brokerFirmId)
        {
            return _httpClient.DeleteAsync($"{ApiEndPoint}/{id}/{BuildQueryString("brokerFirmId", brokerFirmId.ToString())}");
        }

        /// <summary>
        /// Fetches data for a broker. 
        /// </summary>
        /// <param name="id">The ID of the broker to fetch.</param>
        /// <param name="brokerFirmId">The ID of the brokerfirm associated with the broker search.</param>
        /// <returns>A <see cref="Task"/> containing a <see cref="BrokerDto"/> object.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        public Task<BrokerDto?> GetBrokerById([Required] int id, [Required] int brokerFirmId)
        {
            return _httpClient.GetFromJsonAsync<BrokerDto>($"{ApiEndPoint}/{id}/{BuildQueryString("brokerFirmId", brokerFirmId.ToString())}");
        }

        /// <summary>
        /// Fetches data for a broker. 
        /// </summary>
        /// <param name="brokerFirmId">The ID of the brokerfirm associated with the broker search.</param>
        /// <returns>A <see cref="Task"/> containing a <see cref="BrokerDto"/> object.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        public Task<BrokerDto?> GetBrokers([Required] int brokerFirmId)
        {
            return _httpClient.GetFromJsonAsync<BrokerDto>($"{ApiEndPoint}{BuildQueryString("brokerFirmId", brokerFirmId.ToString())}");
        }

        /// <summary>
        /// Updates a broker.
        /// </summary>
        /// <param name="id">The ID of the broker to update.</param>
        /// <param name="broker">The serialized DTO object to send.</param>
        /// <returns>A <see cref="Task"/>.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        public Task UpdateBroker([Required] int id, [Required] UpdateBrokerDto broker)
        {
            return _httpClient.PutAsJsonAsync($"{ApiEndPoint}/{id}", broker);
        }

        #endregion
    }
}

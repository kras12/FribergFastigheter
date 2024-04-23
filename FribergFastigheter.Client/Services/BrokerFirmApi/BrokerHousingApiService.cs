using FribergFastigheter.Shared.Dto;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;

namespace FribergFastigheter.Client.Services.HousingApi
{
    /// <summary>
    /// A service to fetch data from the Friberg Fastigheter Broker Housing API endpoint.
    /// </summary>
    /// <!-- Author: Jimmie -->
    /// <!-- Co Authors: -->
    public class BrokerHousingApiService : FribergApiServiceBase, IBrokerHousingApiService
    {
        #region Constants

        private const string ApiEndPoint = "api/BrokerFirm/Housing";

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="httpClient">The injected HTTP client service.</param>
        public BrokerHousingApiService(FribergApiHttpClientService httpClientService) : base(httpClientService)
        {

        }

        #endregion

        #region OpenAPiMethods

        /// <summary>
        /// Creates a new housing under the broker firm.
        /// </summary>
        /// <param name="brokerFirmId">The ID of the brokerfirm that the housing belongs to.</param>
        /// <param name="housing">The serialized DTO object to send.</param>
        /// <returns>A <see cref="Task"/>.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        public Task CreateHousing([Required] int brokerFirmId, [Required] CreateHousingDto housing)
        {
            return _httpClient.PostAsJsonAsync($"{ApiEndPoint}/{BuildQueryString("brokerFirmId", brokerFirmId.ToString())}", housing);
        }

        /// <summary>
        /// Deletes a housing object.
        /// </summary>
        /// <param name="id">The ID of the housing object to delete.</param>
		/// <param name="brokerFirmId">The ID of the broker firm associated with the housing object.</param>
        /// <returns>A <see cref="Task"/>.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        public Task DeleteHousing(int id, [Required] int brokerFirmId)
        {
            return _httpClient.DeleteAsync($"{ApiEndPoint}/{id}/{BuildQueryString("brokerFirmId", brokerFirmId.ToString())}");
        }

        /// <summary>
        /// Fetches data for a housing object.
        /// </summary>
        /// <param name="id">The ID of the housing to fetch.</param>
        /// <param name="brokerFirmId">The ID of the broker firm associated with the housing object.</param>
        /// <returns>A <see cref="Task"/> containing a <see cref="HousingDto"/> object.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        /// <param name="includeImageData"></param>
        public Task<HousingDto?> GetHousingById([Required] int id, [Required] int brokerFirmId)
        {
            return _httpClient.GetFromJsonAsync<HousingDto>($"{ApiEndPoint}/{id}/{BuildQueryString("brokerFirmId", brokerFirmId.ToString())}");
        }

        /// <summary>
        /// Fetches data for housing objects. 
        /// </summary>
        /// <param name="brokerFirmId">The ID of the brokerfirm associated with the housing objects.</param>
        /// <returns>A <see cref="Task"/> containing a <see cref="HousingDto"/> object.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        public Task<HousingDto?> GetHousings([Required] int brokerFirmId)
        {
            return _httpClient.GetFromJsonAsync<HousingDto>($"{ApiEndPoint}{BuildQueryString("brokerFirmId", brokerFirmId.ToString())}");
        }

        /// <summary>
        /// Updates a housing object.
        /// </summary>
        /// <param name="id">The ID of the housing object to update.</param>
        /// <param name="brokerFirmId">The ID of the brokerfirm associated with the housing objects.</param>
        /// <param name="housing">The serialized DTO object to send.</param>
        /// <returns>A <see cref="Task"/>.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        public Task UpdateHousing([Required] int id, [Required] int brokerFirmId, [Required] UpdateHousingDto housing)
        {
            return _httpClient.PutAsJsonAsync($"{ApiEndPoint}/{id}/{BuildQueryString("brokerFirmId", brokerFirmId.ToString())}", housing);
        }

        #endregion
    }
}

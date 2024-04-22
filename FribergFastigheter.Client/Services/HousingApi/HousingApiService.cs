using FribergFastigheter.Shared.Dto;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;

namespace FribergFastigheter.Client.Services.HousingApi
{
    /// <summary>
    /// A service to fetch data from Friberg Fastigheter Housing API endpoints.
    /// </summary>
    /// <!-- Author: Jimmie -->
    /// <!-- Co Authors: -->
    public class HousingApiService : FribergApiServiceBase, IHousingApiService
    {
        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="httpClientService">The injected HTTP client service.</param>
        public HousingApiService(FribergApiHttpClientService httpClientService) : base(httpClientService)
        {

        }

        #endregion

        #region OpenAPiMethods

        /// <summary>
        /// Fetches all housings that matches the filters and options.
        /// </summary>
        /// <param name="municipalityId">Sets a filter on municipality.</param>
        /// <param name="limitHousings">Sets the max limit of housing objects to return.</param>
        /// <param name="limitImageCountPerHousing">Sets the max limit of images to return per housing object.</param>
        /// <returns>A <see cref="Task"/> containing a collection of <see cref="HousingDto"/>.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        public async Task<List<HousingDto>?> SearchHousings(int? municipalityId = null, int? limitHousings = null, int? limitImageCountPerHousing = null)
        {
            List<KeyValuePair<string, string>> queries = new();

            if (municipalityId != null)
            {
                queries.Add(new KeyValuePair<string, string>("municipalityId", municipalityId.Value.ToString()));
            }

            if (limitHousings != null)
            {
                queries.Add(new KeyValuePair<string, string>("limitHousings", limitHousings.Value.ToString()));
            }

            if (limitImageCountPerHousing != null)
            {
                queries.Add(new KeyValuePair<string, string>("limitImageCountPerHousing", limitImageCountPerHousing.Value.ToString()));
            }

            return await _httpClient.GetFromJsonAsync<List<HousingDto>>($"/api/Housing/Search{BuildQueryString(queries)}");
        }

        #endregion
    }
}

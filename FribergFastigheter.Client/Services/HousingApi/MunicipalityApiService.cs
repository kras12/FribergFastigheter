using FribergFastigheter.Shared.Dto;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;

namespace FribergFastigheter.Client.Services.HousingApi
{
    /// <summary>
    /// A service to fetch data from the Friberg Fastigheter Municipality API endpoint.
    /// </summary>
    /// <!-- Author: Jimmie -->
    /// <!-- Co Authors: -->
    public class MunicipalityApiService : FribergApiServiceBase, IMunicipalityApiService
    {
        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="httpClientService">The injected HTTP client service.</param>
        public MunicipalityApiService(FribergApiHttpClientService httpClientService) : base(httpClientService)
        {

        }

        #endregion

        #region Methods

        /// <summary>
        /// Fetches all municipalities.
        /// </summary>
        /// <returns>A <see cref="Task"/> containing a collection of <see cref="MunicipalityDto"/>.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        public async Task<List<MunicipalityDto>?> GetMunicipalities()
        {
            var test = await _httpClient.GetFromJsonAsync<List<MunicipalityDto>>($"/api/Housing/Municipality");
            return await _httpClient.GetFromJsonAsync<List<MunicipalityDto>>($"/api/Housing/Municipality");
        }

        #endregion
    }
}

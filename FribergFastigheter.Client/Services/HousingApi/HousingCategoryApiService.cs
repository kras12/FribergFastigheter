using FribergFastigheter.Shared.Dto;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;

namespace FribergFastigheter.Client.Services.HousingApi
{
    /// <summary>
    /// A service to fetch data from the Friberg Fastigheter Housing Category API endpoint.
    /// </summary>
    /// <!-- Author: Jimmie -->
    /// <!-- Co Authors: -->
    public class HousingCategoryApiService : FribergApiServiceBase, IHousingCategoryApiService
    {
        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="httpClientService">The injected HTTP client service.</param>
        public HousingCategoryApiService(FribergApiHttpClientService httpClientService) : base(httpClientService)
        {

        }

        #endregion

        #region OpenAPiMethods

        /// <summary>
        /// Fetches all housing categories.
        /// </summary>
        /// <returns>A <see cref="Task"/> containing a collection of <see cref="HousingCategoryDto"/>.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        public async Task<List<HousingCategoryDto>?> GetCategories()
        {
            return await _httpClient.GetFromJsonAsync<List<HousingCategoryDto>>($"/api/Housing/Category");
        }

        #endregion
    }
}

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
        /// <param name="limitHousings">Sets the max limit of housing objects to return.</param>
        /// <param name="limitImageCountPerHousing">Sets the max limit of images to return per housing object.</param>
        /// <param name="municipalityId">Sets a filter on municipality.</param>
        /// <param name="housingCategoryId"></param>
        /// <param name="minPrice">An optional min price filter.</param>
        /// <param name="maxPrice">An optional max price filter.</param>
        /// <param name="minLivingArea">An optional min living area filter.</param>
        /// <returns>A <see cref="Task"/> containing a collection of <see cref="HousingDto"/>.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        /// <param name="maxLivingArea">An optional max living area filter.</param>
        public async Task<List<HousingDto>?> SearchHousings(int? limitHousings = null, int? limitImageCountPerHousing = null, 
            int? municipalityId = null, int? housingCategoryId = null, decimal? minPrice = null,
            decimal? maxPrice = null, double? minLivingArea = null, double? maxLivingArea = null)
        {
            #region Checks

            if (minPrice != null && minPrice < 0)
            {
                throw new ArgumentException("The min price can't be negative.", nameof(minPrice));
            }

            if (maxPrice != null && (maxPrice < 0) || (minPrice != null && maxPrice < minPrice))
            {
                throw new ArgumentException("The max price can't be negative or less than the min price.", nameof(maxPrice));
            }

            if (minLivingArea != null && minLivingArea < 0)
            {
                throw new ArgumentException("The min living area can't be negative.", nameof(minPrice));
            }

            if (maxLivingArea != null && (maxLivingArea < 0) || (minLivingArea != null && maxLivingArea < minLivingArea))
            {
                throw new ArgumentException("The max living area can't be negative or less than the min living area.", nameof(maxLivingArea));
            }

            #endregion

            List<KeyValuePair<string, string>> queries = new();

            if (municipalityId != null)
            {
                queries.Add(new KeyValuePair<string, string>("municipalityId", municipalityId.Value.ToString()));
            }

            if (housingCategoryId != null)
            {
                queries.Add(new KeyValuePair<string, string>("housingCategoryId", housingCategoryId.Value.ToString()));
            }

            if (limitHousings != null)
            {
                queries.Add(new KeyValuePair<string, string>("limitHousings", limitHousings.Value.ToString()));
            }

            if (limitImageCountPerHousing != null)
            {
                queries.Add(new KeyValuePair<string, string>("limitImageCountPerHousing", limitImageCountPerHousing.Value.ToString()));
            }

            if (minPrice != null)
            {
                queries.Add(new KeyValuePair<string, string>("minPrice", minPrice.Value.ToString()));
            }

            if (maxPrice != null)
            {
                queries.Add(new KeyValuePair<string, string>("maxPrice", maxPrice.Value.ToString()));
            }

            if (minLivingArea != null)
            {
                queries.Add(new KeyValuePair<string, string>("minLivingArea", minLivingArea.Value.ToString()));
            }

            if (maxLivingArea != null)
            {
                queries.Add(new KeyValuePair<string, string>("maxLivingArea", maxLivingArea.Value.ToString()));
            }

            return await _httpClient.GetFromJsonAsync<List<HousingDto>>($"/api/Housing/Search{BuildQueryString(queries)}");
        }

        #endregion
    }
}

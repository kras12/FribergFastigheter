using FribergFastigheter.Shared.Dto;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;

namespace FribergFastigheter.Client.Services.FribergFastigheterApi
{
    /// <summary>
    /// A service to fetch data from Friberg Fastigheter Housing API endpoints.
    /// </summary>
    /// <!-- Author: Jimmie -->
    /// <!-- Co Authors: -->
    public class HousingApiService : ApiServiceBase, IHousingApiService
    {
        #region Constants

        /// <summary>
        /// The broker API endpoint address.
        /// </summary>
        private const string BrokerApiEndpoint = "api/Housing/Broker";

        /// <summary>
        /// The broker firm API endpoint address.
        /// </summary>
        private const string BrokerFirmApiEndpoint = "api/Housing/BrokerFirm";

        /// <summary>
        /// The housing category API endpoint address.
        /// </summary>
        private const string HousingCategoryApiEndpoint = "api/Housing/Category";

        /// <summary>
        /// The housing search API endpoint address.
        /// </summary>
        private const string HousingSearchApiEndoint = "api/Housing/Search";

        /// <summary>
        /// The municipality API endpoint address.
        /// </summary>
        private const string MunicipalityApiEndpoint = "api/Housing/Municipality";

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="httpClient">The injected HTTP client.</param>
        public HousingApiService(HttpClient httpClient) : base(httpClient)
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
            return await _httpClient.GetFromJsonAsync<BrokerDto>($"{BrokerApiEndpoint}/{id}");
        }

        /// <summary>
        /// Fetches data for a broker firm. 
        /// </summary>
        /// <param name="id">The ID of the broker firm.</param>
        /// <returns>A <see cref="Task"/> containing a <see cref="BrokerFirmDto"/> object.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        public async Task<BrokerFirmDto?> GetBrokerFirmById(int id)
        {
            return await _httpClient.GetFromJsonAsync<BrokerFirmDto>($"{BrokerFirmApiEndpoint}/{id}");
        }

        /// <summary>
        /// Fetches a housing object by ID.
        /// </summary>
        /// <param name="housingId">The ID of the housing object.</param>
        /// <returns>A <see cref="Task"/> containing a <see cref="HousingDto"/> object.</returns>
        public async Task<HousingDto?> GetHousingById(int housingId)
        {
            return await _httpClient.GetFromJsonAsync<HousingDto?>($"{HousingSearchApiEndoint}/{housingId}");
        }

        /// <summary>
        /// Fetches all housing categories.
        /// </summary>
        /// <returns>A <see cref="Task"/> containing a collection of <see cref="HousingCategoryDto"/>.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        public async Task<List<HousingCategoryDto>?> GetHousingCategories()
        {
            return await _httpClient.GetFromJsonAsync<List<HousingCategoryDto>>($"{HousingCategoryApiEndpoint}");
        }

        /// <summary>
        /// Fetches all municipalities.
        /// </summary>
        /// <returns>A <see cref="Task"/> containing a collection of <see cref="MunicipalityDto"/>.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        public async Task<List<MunicipalityDto>?> GetMunicipalities()
        {
            return await _httpClient.GetFromJsonAsync<List<MunicipalityDto>>($"{MunicipalityApiEndpoint}");
        }

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
        /// <param name="maxLivingArea">An optional max living area filter.</param>
        /// <param name="offsetRows">An optional number of rows to skip.</param>
        /// <returns>A <see cref="Task"/> containing a <see cref="HousingSearchResultDto"/> object if successful.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        public async Task<HousingSearchResultDto?> SearchHousings(int? limitHousings = null, int? limitImageCountPerHousing = null,
            int? municipalityId = null, int? housingCategoryId = null, decimal? minPrice = null,
            decimal? maxPrice = null, double? minLivingArea = null, double? maxLivingArea = null, int? offsetRows = null)
        {
            #region Checks

            if (minPrice != null && minPrice < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(minPrice), "The min price can't be negative.");
            }

            if (maxPrice != null && maxPrice < 0 || minPrice != null && maxPrice < minPrice)
            {
                throw new ArgumentOutOfRangeException(nameof(maxPrice), "The max price can't be negative or less than the min price.");
            }

            if (minLivingArea != null && minLivingArea < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(minPrice), "The min living area can't be negative.");
            }

            if (maxLivingArea != null && maxLivingArea < 0 || minLivingArea != null && maxLivingArea < minLivingArea)
            {
                throw new ArgumentOutOfRangeException(nameof(maxLivingArea), "The max living area can't be negative or less than the min living area.");
            }

            if (offsetRows != null && offsetRows < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(offsetRows), "The offset rows value can't be negative.");
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

            if (offsetRows != null)
            {
                queries.Add(new KeyValuePair<string, string>("offsetRows", offsetRows.Value.ToString()));
            }

            return await _httpClient.GetFromJsonAsync<HousingSearchResultDto>($"{HousingSearchApiEndoint}{BuildQueryString(queries)}");
        }

        #endregion
    }
}

using FribergFastigheter.Shared.Dto.Api;
using FribergFastigheter.Shared.Dto.Broker;
using FribergFastigheter.Shared.Dto.BrokerFirm;
using FribergFastigheter.Shared.Dto.Housing;
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
        #region BrokerConstants

        /// <summary>
        /// The relative broker by ID API endpoint address.
        /// </summary>
        private const string BrokerByIdApiEndpoint = $"{ApiBase}/broker/{IdPlaceHolder}";

		/// <summary>
		/// The brokers API endpoint address.
		/// </summary>
		private const string BrokersByBrokerFirmIdApiEndPoint = $"{ApiBase}/brokers";

		#endregion

		#region BrokerFirmConstants

		/// <summary>
		/// The relative broker firm API endpoint address.
		/// </summary>
		private const string BrokerFirmByIdApiEndpoint = $"{ApiBase}/broker-firm/{IdPlaceHolder}";

        #endregion

        #region HousingContants

        /// <summary>
		/// The relative housing search API endpoint address.
		/// </summary>
		private const string HousingByIdApiEndoint = $"{ApiBase}/housing/{IdPlaceHolder}";

        /// <summary>
        /// The relative housing category list API endpoint address.
        /// </summary>
        private const string HousingCategoryListApiEndpoint = $"{ApiBase}/housing/categories";

        /// <summary>
        /// The relative housing API endpoint address.
        /// </summary>
        private const string HousingApiEndoint = $"{ApiBase}/housings";

        /// <summary>
        /// The relative housing search API endpoint address.
        /// </summary>
        private const string HousingSearchApiEndpoint = $"{ApiBase}/housings/search";

		/// <summary>
		/// The relative municipality list API endpoint address.
		/// </summary>
		private const string MunicipalityListApiEndpoint = $"{ApiBase}/municipalities";

        #endregion

        #region Constants

        /// <summary>
        /// The broker API base address.
        /// </summary>
        private const string ApiBase = "housing-api"; 		

		/// <summary>
		/// The ID placeholder used in API endpoint addresses.
		/// </summary>
		private const string IdPlaceHolder = "{id}";		

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
		/// Fetches a broker by ID.
		/// </summary>
		/// <param name="id">The ID of the broker.</param>
		/// <returns>A <see cref="Task"/> containing a <see cref="BrokerDto"/> object if successful.</returns>
		/// <!-- Author: Jimmie -->
		/// <!-- Co Authors: -->
		public async Task<ApiResponseDto<BrokerDto>> GetBrokerById(int id)
        {
            var result = await _httpClient.GetFromJsonAsync<ApiResponseDto<BrokerDto>>(BrokerByIdApiEndpoint.Replace(IdPlaceHolder, id.ToString()));
            return EnsureNotNull(result, "Failed to serialize the API response.");
        }

		/// <summary>
		/// Fetches data for a broker. 
		/// </summary>
		/// <returns>A <see cref="Task"/> containing a <see cref="BrokerDto"/> object.</returns>
		/// <!-- Author: Jimmie  -->
		/// <!-- Co Authors: Marcus -->
		public async Task<ApiResponseDto<List<BrokerDto>>> GetBrokers(int brokerFirmId)
		{
            var result = await _httpClient.GetFromJsonAsync<ApiResponseDto<List<BrokerDto>>>($"{BrokersByBrokerFirmIdApiEndPoint}{BuildQueryString("brokerFirmId", brokerFirmId.ToString())}");
            return EnsureNotNull(result, "Failed to serialize the API response.");
        }

        /// <summary>
        /// Fetches a broker firm by ID.
        /// </summary>
        /// <param name="id">The ID of the broker firm.</param>
        /// <returns>A <see cref="Task"/> containing a <see cref="BrokerFirmDto"/> object.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        public async Task<ApiResponseDto<BrokerFirmDto>> GetBrokerFirmById(int id)
        {
            var result = await _httpClient.GetFromJsonAsync<ApiResponseDto<BrokerFirmDto>>(BrokerFirmByIdApiEndpoint.Replace(IdPlaceHolder, id.ToString()));
            return EnsureNotNull(result, "Failed to serialize the API response.");
        }

        /// <summary>
        /// Fetches all housing objects for a broker firm.
        /// </summary>
        /// <param name="brokerFirmId">The ID of the broker firm to filter by.</param>
        /// <param name="brokerId">The ID of the broker to filter by.</param>
        /// <param name="limitImagesPerHousing">Sets the max limit of images to return per housing object.</param>
        /// <returns>A <see cref="Task"/> containing a <see cref="HousingDto"/> object.</returns>
        /// <!-- Author: Jimmie  -->
        /// <!-- Co Authors: Marcus -->
        private async Task<ApiResponseDto<List<HousingDto>>> GetHousings(int? brokerFirmId = null, int? brokerId = null, int? limitImagesPerHousing = null)
        {
            List<KeyValuePair<string, string>> queries = new();

            if (brokerFirmId != null)
            {
                queries.Add(new KeyValuePair<string, string>("brokerFirmId", brokerFirmId.ToString()!));
            }

            if (brokerId != null)
            {
                queries.Add(new KeyValuePair<string, string>("brokerId", brokerId.ToString()!));
            }

            if (limitImagesPerHousing != null)
            {
                queries.Add(new KeyValuePair<string, string>("limitImagesPerHousing", limitImagesPerHousing.Value.ToString()));
            }

            var result = await _httpClient.GetFromJsonAsync<ApiResponseDto<List<HousingDto>>>($"{HousingApiEndoint}{BuildQueryString(queries)}");
            return EnsureNotNull(result, "Failed to serialize the API response.");
        }

        /// <summary>
        /// Fetches housing objects that are assignt to a specific broker.
        /// </summary>
        /// <param name="brokerId">The ID of the broker.</param>
        /// <param name="limitImagesPerHousing">Sets the max limit of images to return per housing object.</param>
        /// <returns>A <see cref="Task"/> containing a collection of <see cref="HousingDto"/> objects if successful.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        public Task<ApiResponseDto<List<HousingDto>>> GetHousingsByBroker(int brokerId, int? limitImagesPerHousing = null)
		{
            return GetHousings(brokerId: brokerId, limitImagesPerHousing: limitImagesPerHousing);
        }

        /// <summary>
        /// Fetches all housing objects for a broker firm.
        /// </summary>
        /// <param name="brokerFirmId">The ID of the broker firm.</param>
        /// <param name="limitImagesPerHousing">Sets the max limit of images to return per housing object.</param>
        /// <returns>A <see cref="Task"/> containing a collection of <see cref="HousingDto"/> objects if successful.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        public Task<ApiResponseDto<List<HousingDto>>> GetHousingsByBrokerFirm(int brokerFirmId, int? limitImagesPerHousing = null)
        {
            return GetHousings(brokerFirmId: brokerFirmId, limitImagesPerHousing: limitImagesPerHousing);
        }

        /// <summary>
        /// Fetches a housing object by ID.
        /// </summary>
        /// <param name="housingId">The ID of the housing object.</param>
        /// <returns>A <see cref="Task"/> containing a <see cref="HousingDto"/> object.</returns>
        public async Task<ApiResponseDto<HousingDto>> GetHousingById(int housingId)
        {
            var result = await _httpClient.GetFromJsonAsync<ApiResponseDto<HousingDto>>(HousingByIdApiEndoint.Replace(IdPlaceHolder, housingId.ToString()));
            return EnsureNotNull(result, "Failed to serialize the API response.");
        }        

		/// <summary>
		/// Fetches all housing categories.
		/// </summary>
		/// <returns>A <see cref="Task"/> containing a collection of <see cref="HousingCategoryDto"/>.</returns>
		/// <!-- Author: Jimmie -->
		/// <!-- Co Authors: -->
		public async Task<ApiResponseDto<List<HousingCategoryDto>>> GetHousingCategories()
        {
            var result = await _httpClient.GetFromJsonAsync<ApiResponseDto<List<HousingCategoryDto>>>(HousingCategoryListApiEndpoint);
            return EnsureNotNull(result, "Failed to serialize the API response.");
        }

        /// <summary>
        /// Fetches all municipalities.
        /// </summary>
        /// <returns>A <see cref="Task"/> containing a collection of <see cref="MunicipalityDto"/>.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        public async Task<ApiResponseDto<List<MunicipalityDto>>> GetMunicipalities()
        {
            var result = await _httpClient.GetFromJsonAsync<ApiResponseDto<List<MunicipalityDto>>>(MunicipalityListApiEndpoint);
            return EnsureNotNull(result, "Failed to serialize the API response.");
        }

        /// <summary>
        /// Fetches all housings that matches the filters and options.
        /// </summary>
        /// <param name="limitHousings">Sets the max limit of housing objects to return.</param>
        /// <param name="limitImagesPerHousing">Sets the max limit of images to return per housing object.</param>
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
        public async Task<ApiResponseDto<HousingSearchResultDto>> SearchHousings(int? limitHousings = null, int? limitImagesPerHousing = null,
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

            if (limitImagesPerHousing != null)
            {
                queries.Add(new KeyValuePair<string, string>("limitImagesPerHousing", limitImagesPerHousing.Value.ToString()));
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

            var result = await _httpClient.GetFromJsonAsync<ApiResponseDto<HousingSearchResultDto>>($"{HousingSearchApiEndpoint}{BuildQueryString(queries)}");
            return EnsureNotNull(result, "Failed to serialize the API response.");
        }

        #endregion
    }
}

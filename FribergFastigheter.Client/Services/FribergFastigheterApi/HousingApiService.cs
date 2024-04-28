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
        /// The relative housings by broker ID API endpoint address.
        /// </summary>
        private const string HousingsByBrokerApiEndpoint = $"api/Housing/Broker/{IdPlaceHolder}/Housing";

		/// <summary>
		/// The relative broker API endpoint address.
		/// </summary>
		private const string BrokerByIdApiEndpoint = $"api/Housing/Broker/{IdPlaceHolder}";

		/// <summary>
		/// The relative broker firm API endpoint address.
		/// </summary>
		private const string BrokerFirmByIdApiEndpoint = $"api/Housing/BrokerFirm/{IdPlaceHolder}";

		/// <summary>
		/// The relative housing search API endpoint address.
		/// </summary>
		private const string HousingByIdApiEndoint = $"api/Housing/{IdPlaceHolder}";

		/// <summary>
		/// The relative housing API endpoint address.
		/// </summary>
		private const string HousingByIdBrokerApiEndPoint = $"api/Housing/{IdPlaceHolder}/Broker";

		/// <summary>
		/// The relative housing category list API endpoint address.
		/// </summary>
		private const string HousingCategoryListApiEndpoint = "api/Housing/Category";

		/// <summary>
		/// The relative housing search API endpoint address.
		/// </summary>
		private const string HousingSearchApiEndoint = "api/Housing/Search";

		/// <summary>
		/// The ID placeholder used in API endpoint addresses.
		/// </summary>
		private const string IdPlaceHolder = "{id}";

		/// <summary>
		/// The relative municipality list API endpoint address.
		/// </summary>
		private const string MunicipalityListApiEndpoint = "api/Housing/Municipality";

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
		/// Fetches data for a broker associated with a housing object.
		/// </summary>
		/// <param name="id">The ID of the housing object.</param>
		/// <returns>A <see cref="Task"/> containing a <see cref="BrokerDto"/> object if successful.</returns>
		/// <!-- Author: Jimmie -->
		/// <!-- Co Authors: -->
		public async Task<BrokerDto?> GetBrokerByHousingId(int id)
		{
			return await _httpClient.GetFromJsonAsync<BrokerDto>(HousingByIdBrokerApiEndPoint.Replace(IdPlaceHolder, id.ToString()));
		}

		/// <summary>
		/// Fetches data for a broker. 
		/// </summary>
		/// <param name="id">The ID of the broker.</param>
		/// <returns>A <see cref="Task"/> containing a <see cref="BrokerDto"/> object if successful.</returns>
		/// <!-- Author: Jimmie -->
		/// <!-- Co Authors: -->
		public async Task<BrokerDto?> GetBrokerById(int id)
        {
            return await _httpClient.GetFromJsonAsync<BrokerDto>(BrokerByIdApiEndpoint.Replace(IdPlaceHolder, id.ToString()));
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
            return await _httpClient.GetFromJsonAsync<BrokerFirmDto>(BrokerFirmByIdApiEndpoint.Replace(IdPlaceHolder, id.ToString()));
        }

		/// <summary>
		/// Fetches housing objects that is being handled by a broker.
		/// </summary>
		/// <param name="brokerId">The ID of the broker.</param>
		/// <param name="limitImagesPerHousing">Sets the max limit of images to return per housing object.</param>
		/// <returns>A <see cref="Task"/> containing a collection of <see cref="HousingDto"/> objects if successful.</returns>
		public async Task<List<HousingDto>?> GetHousingsByBrokerId(int brokerId, int? limitImagesPerHousing = null)
		{
			List<KeyValuePair<string, string>> queries = new();

			if (limitImagesPerHousing != null)
			{
				queries.Add(new KeyValuePair<string, string>("limitImagesPerHousing", limitImagesPerHousing.Value.ToString()));
			}

			return await _httpClient.GetFromJsonAsync<List<HousingDto>?>($"{HousingsByBrokerApiEndpoint.Replace(IdPlaceHolder, brokerId.ToString())}{BuildQueryString(queries)}");
		}

		/// <summary>
		/// Fetches a housing object by ID.
		/// </summary>
		/// <param name="housingId">The ID of the housing object.</param>
		/// <returns>A <see cref="Task"/> containing a <see cref="HousingDto"/> object.</returns>
		public async Task<HousingDto?> GetHousingById(int housingId)
        {
            return await _httpClient.GetFromJsonAsync<HousingDto?>(HousingByIdApiEndoint.Replace(IdPlaceHolder, housingId.ToString()));
        }

        /// <summary>
        /// Fetches all housing categories.
        /// </summary>
        /// <returns>A <see cref="Task"/> containing a collection of <see cref="HousingCategoryDto"/>.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        public async Task<List<HousingCategoryDto>?> GetHousingCategories()
        {
            return await _httpClient.GetFromJsonAsync<List<HousingCategoryDto>>($"{HousingCategoryListApiEndpoint}");
        }

        /// <summary>
        /// Fetches all municipalities.
        /// </summary>
        /// <returns>A <see cref="Task"/> containing a collection of <see cref="MunicipalityDto"/>.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        public async Task<List<MunicipalityDto>?> GetMunicipalities()
        {
            return await _httpClient.GetFromJsonAsync<List<MunicipalityDto>>($"{MunicipalityListApiEndpoint}");
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
        public async Task<HousingSearchResultDto?> SearchHousings(int? limitHousings = null, int? limitImagesPerHousing = null,
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

            return await _httpClient.GetFromJsonAsync<HousingSearchResultDto>($"{HousingSearchApiEndoint}{BuildQueryString(queries)}");
        }

        #endregion
    }
}

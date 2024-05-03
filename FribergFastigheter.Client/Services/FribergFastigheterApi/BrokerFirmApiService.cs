using FribergFastigheter.Shared.Dto;
using FribergFastigheter.Shared.Dto.Statistics;
using Microsoft.AspNetCore.Components.Forms;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FribergFastigheter.Client.Services.FribergFastigheterApi
{
    /// <summary>
    /// A service to fetch data from the Friberg Fastigheter Broker Firm API.
    /// </summary>
    /// <!-- Author: Jimmie -->
    /// <!-- Co Authors: -->
    public class BrokerFirmApiService : ApiServiceBase, IBrokerFirmApiService
    {
        #region Constants

        /// <summary>
        /// The broker API endpoint address.
        /// </summary>
        private const string BrokerApiEndPoint = "api/BrokerFirm/Broker";

		/// <summary>
		/// The broker API endpoint address.
		/// </summary>
		private const string BrokerByIdApiEndPoint = $"api/BrokerFirm/Broker/{IdPlaceHolder}";

		/// <summary>
		/// The broker firm API endpoint address.
		/// </summary>
		private const string BrokerFirmByIdApiEndPoint = $"api/BrokerFirm/{IdPlaceHolder}";

        /// <summary>
		/// The broker firm API endpoint address.
		/// </summary>
		private const string BrokerFirmStatisticsApiEndPoint = $"api/BrokerFirm/{IdPlaceHolder}/Statistics";

        // <summary>
        /// The housing API endpoint address.
        /// </summary>
        private const string HousingApiEndPoint = $"api/BrokerFirm/Housing/BrokerFirm/{IdPlaceHolder}/Housing";

		// <summary>
		/// The housing API endpoint address.
		/// </summary>
		private const string HousingByIdApiEndPoint = $"api/BrokerFirm/Housing/{IdPlaceHolder}";

        /// <summary>
		/// The relative housing category list API endpoint address.
		/// </summary>
		private const string HousingCategoryListApiEndpoint = "api/BrokerFirm/Housing/Category";

        // <summary>
        /// The housing image API endpoint address.
        /// </summary>
        private const string HousingImageApiEndPoint = "api/BrokerFirm/Housing/Image";

		// <summary>
		/// The housing image API endpoint address.
		/// </summary>
		private const string HousingImageByIdApiEndPoint = $"api/BrokerFirm/Housing/Image/{IdPlaceHolder}";

		/// <summary>
		/// The ID placeholder used in API endpoint addresses.
		/// </summary>
		private const string IdPlaceHolder = "{id}";

        /// <summary>
        /// The relative housings by broker ID API endpoint address.
        /// </summary>
        private const string HousingsByBrokerApiEndpoint = $"api/BrokerFirm/Housing/Broker/{IdPlaceHolder}/Housing";

        /// <summary>
		/// The relative municipality list API endpoint address.
		/// </summary>
		private const string MunicipalityListApiEndpoint = "api/BrokerFirm/Housing/Municipality";

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="httpClient">The injected HTTP client.</param>
        public BrokerFirmApiService(HttpClient httpClient) : base(httpClient)
        {

        }

        #endregion

        #region BrokerMethods

        public async Task<BrokerDto?> CreateBroker([Required] int brokerFirmId, [Required] CreateBrokerDto broker)
        {
            List<KeyValuePair<string, string>> queries = new()
            {
                new KeyValuePair<string, string>("brokerFirmId", brokerFirmId.ToString()),
                new KeyValuePair<string, string>("returnCreatedBroker", true.ToString())
            };

            var response = await _httpClient.PostAsJsonAsync($"{BrokerApiEndPoint}/{BuildQueryString(queries)}", broker);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<BrokerDto>(new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
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
            return _httpClient.DeleteAsync($"{BrokerByIdApiEndPoint.Replace(IdPlaceHolder, id.ToString())}{BuildQueryString("brokerFirmId", brokerFirmId.ToString())}");
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
            return _httpClient.GetFromJsonAsync<BrokerDto>($"{BrokerByIdApiEndPoint.Replace(IdPlaceHolder, id.ToString())}{BuildQueryString("brokerFirmId", brokerFirmId.ToString())}");
        }

        /// <summary>
        /// Fetches data for a broker. 
        /// </summary>
        /// <param name="brokerFirmId">The ID of the brokerfirm associated with the broker search.</param>
        /// <returns>A <see cref="Task"/> containing a <see cref="BrokerDto"/> object.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        public Task<List<BrokerDto>?> GetBrokers([Required] int brokerFirmId)
        {
            return _httpClient.GetFromJsonAsync<List<BrokerDto>?>($"{BrokerApiEndPoint}{BuildQueryString("brokerFirmId", brokerFirmId.ToString())}");
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
            return _httpClient.PutAsJsonAsync(BrokerByIdApiEndPoint.Replace(IdPlaceHolder, id.ToString()), broker);
        }

		#endregion

		#region BrokerFirmMethods

		/// <summary>
		/// Fetches a broker firm.
		/// </summary>
		/// <param name="brokerFirmId">The ID of the brokerfirm.</param>
		/// <returns>A <see cref="Task"/> containing a <see cref="BrokerFirmDto"/> object.</returns>
		/// <!-- Author: Jimmie -->
		/// <!-- Co Authors: -->
		public Task<BrokerFirmDto?> GetBrokerFirmById([Required] int brokerFirmId)
        {
            return _httpClient.GetFromJsonAsync<BrokerFirmDto>(BrokerFirmByIdApiEndPoint.Replace(IdPlaceHolder, brokerFirmId.ToString()));
        }

        /// <summary>
        /// Fetches statistics for a broker firm.
        /// </summary>
        /// <param name="brokerFirmId">The ID of the broker firm.</param>
        /// <returns>A <see cref="Task"/> containing a <see cref="BrokerFirmStatisticsViewModel"/> object.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        public Task<BrokerFirmStatisticsViewModel?> GetBrokerFirmStatistics([Required] int brokerFirmId)
        {
            return _httpClient.GetFromJsonAsync<BrokerFirmStatisticsViewModel>(BrokerFirmStatisticsApiEndPoint.Replace(IdPlaceHolder, brokerFirmId.ToString()));
        }

        #endregion

        #region HousingMethods

        /// <summary>
        /// Creates a new housing under the broker firm.
        /// </summary>
        /// <param name="brokerFirmId">The ID of the brokerfirm that the housing belongs to.</param>
        /// <param name="housing">The serialized DTO object to send.</param>
        /// <returns>A <see cref="Task"/> containg a <see cref="HousingDto"/> object if successful.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        public async Task<HousingDto?> CreateHousing([Required] int brokerFirmId, [Required] CreateHousingDto housing)
        {
            List<KeyValuePair<string, string>> queries = new()
            {
                new KeyValuePair<string, string>("brokerFirmId", brokerFirmId.ToString()),
                new KeyValuePair<string, string>("returnCreatedHousing", true.ToString())
            };

            var response = await _httpClient.PostAsJsonAsync($"{HousingApiEndPoint.Replace(IdPlaceHolder, housing.BrokerFirmId.ToString())}/{BuildQueryString(queries)}", housing);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<HousingDto>(new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }

        /// <summary>
        /// Deletes a housing object.
        /// </summary>
        /// <param name="brokerFirmId">The ID of the broker firm associated with the housing object.</param>
        /// <param name="housingId">The ID of the housing object to fetch images for.</param>
        /// <param name="imageIds">Contains the IDs of the images to delete.</param>
        /// <returns>A <see cref="Task"/>.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        public async Task DeleteHousing([Required] int brokerFirmId, int housingId, List<int> imageIds)
        {
            if (imageIds.Count > 0)
            {
                await DeleteImages(brokerFirmId, housingId, imageIds);
            }

            await _httpClient.DeleteAsync($"{HousingByIdApiEndPoint.Replace(IdPlaceHolder, housingId.ToString())}{BuildQueryString("brokerFirmId", brokerFirmId.ToString())}");
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
            return _httpClient.GetFromJsonAsync<HousingDto>($"{HousingByIdApiEndPoint.Replace(IdPlaceHolder, id.ToString())}{BuildQueryString("brokerFirmId", brokerFirmId.ToString())}");
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

        /// <returns>A <see cref="Task"/> containing a <see cref="HousingDto"/> object.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        public Task<List<HousingDto>?> GetHousings([Required] int brokerFirmId, int? limitImagesPerHousing = null)
        {
            List<KeyValuePair<string, string>> queries = new();

            queries.Add(new KeyValuePair<string, string>("brokerFirmId", brokerFirmId.ToString()));
            if (limitImagesPerHousing != null)
            {
                queries.Add(new KeyValuePair<string, string>("limitImagesPerHousing", limitImagesPerHousing.Value.ToString()));
            }

            return _httpClient.GetFromJsonAsync<List<HousingDto>?>($"{HousingApiEndPoint.Replace(IdPlaceHolder, brokerFirmId.ToString())}{BuildQueryString(queries)}");
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
        /// Updates a housing object.
        /// </summary>
        /// <param name="housing">The serialized DTO object to send.</param>
        /// <returns>A <see cref="Task"/>.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        public Task UpdateHousing([Required] EditHousingDto housing)
        {
            return _httpClient.PutAsJsonAsync($"{HousingByIdApiEndPoint.Replace(IdPlaceHolder, housing.HousingId.ToString())}{BuildQueryString("brokerFirmId", housing.BrokerFirmId.ToString())}", housing);
        }

        /// <summary>
		/// Fetches housing objects that is being handled by a broker.
		/// </summary>
		/// <param name="brokerId">The ID of the broker.</param>
		/// <param name="limitImagesPerHousing">Sets the max limit of images to return per housing object.</param>
		/// <returns>A <see cref="Task"/> containing a collection of <see cref="HousingDto"/> objects if successful.</returns>
		public async Task<List<HousingDto>?> GetHousingsByBrokerId(int brokerId, int brokerFirmId, int? limitImagesPerHousing = null)
        {
            List<KeyValuePair<string, string>> queries = new();

            queries.Add(new KeyValuePair<string, string>("brokerFirmId", brokerFirmId.ToString()));
            if (limitImagesPerHousing != null)
            {
                queries.Add(new KeyValuePair<string, string>("limitImagesPerHousing", limitImagesPerHousing.Value.ToString()));
            }

            return await _httpClient.GetFromJsonAsync<List<HousingDto>?>($"{HousingsByBrokerApiEndpoint.Replace(IdPlaceHolder, brokerId.ToString())}{BuildQueryString(queries)}");
        }

        #endregion

        #region HousingImageMethods

        /// <summary>
        /// Fetches all images for a housing object. 
        /// </summary>
        /// <param name="brokerFirmId">The broker firm the housing object belongs to.</param>
        /// <param name="housingId">The ID of the housing object to fetch images for. </param>
        /// <returns>A <see cref="Task"/> containing a collection of <see cref="HousingDto"/> objects.</returns>
        public Task<List<ImageDto>?> GetImages(int brokerFirmId, int housingId)
        {
            List<KeyValuePair<string, string>> queries = new()
            {
                new KeyValuePair<string, string>("brokerFirmId", brokerFirmId.ToString()),
                new KeyValuePair<string, string>("housingId", housingId.ToString())
            };

            return _httpClient.GetFromJsonAsync<List<ImageDto>>($"{HousingImageApiEndPoint}{BuildQueryString(queries)}");
        }

        /// <summary>
        /// Deletes an image for a housing object.
        /// </summary>
        /// <param name="id">The ID of the image object to delete.</param>
		/// <param name="brokerFirmId">The ID of the broker firm associated with the housing object the image belongs to.</param>
        /// <param name="housingId">The ID of the housing object the image belongs to</param>
        /// <returns>A <see cref="Task"/>.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        public Task DeleteImage(int id, [Required] int brokerFirmId, [Required] int housingId)
        {
            List<KeyValuePair<string, string>> queries = new()
            {
                new KeyValuePair<string, string>("brokerFirmId", brokerFirmId.ToString()),
                new KeyValuePair<string, string>("housingId", housingId.ToString())
            };

            return _httpClient.DeleteAsync($"{HousingImageByIdApiEndPoint.Replace(IdPlaceHolder, id.ToString())}{BuildQueryString(queries)}");
        }

        /// <summary>
        /// Deletes an image for a housing object.
        /// </summary>
        /// <param name="brokerFirmId">The ID of the broker firm associated with the housing object that owns the images.</param>
        /// <param name="housingId">The ID of the housing object that owns the images. </param>
        /// <param name="imageIds">Contains the IDs of the images to delete.</param>
        /// <returns>A <see cref="Task"/>.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        public Task DeleteImages([Required] int brokerFirmId, int housingId, List<int> imageIds)
        {
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Delete,
                $"{HousingImageApiEndPoint}{BuildQueryString("brokerFirmId", brokerFirmId.ToString())}")
            {
                Content = JsonContent.Create(new DeleteImagesDto(housingId, imageIds))
            };

            return _httpClient.SendAsync(requestMessage);
        }      

        /// <summary>
        /// Uploads images for a housing object. 
        /// </summary>
        /// <param name="brokerFirmId">The ID of the broker firm associated with the housing object the image belongs to.</param>
        /// <param name="housingId">The ID of the housing object the image belongs to</param>
        /// <param name="newFiles">A collection of files to upload.</param>
        /// <returns>A <see cref="Task"/> containing a collection of <see cref="ImageDto"/> objects for the uploaded images.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        public async Task<List<ImageDto>> UploadImages([Required] int brokerFirmId, [Required] int housingId, List<IBrowserFile> newFiles)
        {
            if (newFiles.Count == 0)
            {
                throw new ArgumentException($"The collection '{newFiles}' can't be empty.", nameof(newFiles));
            }

            List<KeyValuePair<string, string>> queries = new()
            {
                new KeyValuePair<string, string>("brokerFirmId", brokerFirmId.ToString()),
                new KeyValuePair<string, string>("housingId", housingId.ToString())
            };

            var content = new MultipartFormDataContent();

            foreach (var file in newFiles)
            {
                content.Add(new StreamContent(file.OpenReadStream()), "files", file.Name);
            }

            var response = await _httpClient.PostAsync($"{HousingImageApiEndPoint}/{BuildQueryString(queries)}", content);
            response.EnsureSuccessStatusCode();
            return (await response.Content.ReadFromJsonAsync<List<ImageDto>>(new JsonSerializerOptions() { PropertyNameCaseInsensitive = true }))!;
        }

        #endregion
    }
}

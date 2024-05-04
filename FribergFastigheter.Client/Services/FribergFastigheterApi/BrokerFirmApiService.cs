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

        #region BrokerApiConstants

        /// <summary>
        /// The broker API endpoint address.
        /// </summary>
        private const string BrokersApiEndPoint = $"{ApiBase}/brokers";

        /// <summary>
        /// The broker API endpoint address.
        /// </summary>
        private const string BrokerByIdApiEndPoint = $"{ApiBase}/broker/{IdPlaceHolder}";

        // <summary>
        /// The broker profile image API endpoint address.
        /// </summary>
        private const string BrokerImageByIdApiEndPoint = $"{ApiBase}/broker/{IdPlaceHolder}/profile-image";

        // <summary>
        /// The broker image API endpoint address.
        /// </summary>
        private const string BrokerProfileImageApiEndPoint = $"{ApiBase}/broker/profile-image";

        #endregion

        #region BrokerFirmApiConstants

        /// <summary>
        /// The broker firm API endpoint address.
        /// </summary>
        private const string BrokerFirmByIdApiEndPoint = $"{ApiBase}/firm/{IdPlaceHolder}";

        /// <summary>
		/// The broker firm API endpoint address.
		/// </summary>
		private const string BrokerFirmStatisticsApiEndPoint = $"{ApiBase}/firm/{IdPlaceHolder}/statistics";

        #endregion

        #region HousingApiConstants

        // <summary>
        /// The housing API endpoint address.
        /// </summary>
        private const string HousingApiEndPoint = $"{ApiBase}/housings";

        // <summary>
        /// The housing API endpoint address.
        /// </summary>
        private const string HousingByIdApiEndPoint = $"{ApiBase}/housing/{IdPlaceHolder}";

        /// <summary>
		/// The relative housing category list API endpoint address.
		/// </summary>
		private const string HousingCategoryListApiEndpoint = $"{ApiBase}/housing-categories";

        /// <summary>
        /// The relative housing Count by broker ID API endpoint address.
        /// </summary>
        private const string HousingCountByBrokerApiEndpoint = $"{ApiBase}/housings/count";

        // <summary>
        /// The housing image API endpoint address.
        /// </summary>
        private const string HousingImageApiEndPoint = $"{ApiBase}/housing/{IdPlaceHolder}/images";

        // <summary>
        /// The housing image API endpoint address.
        /// </summary>
        private const string HousingImageByIdApiEndPoint = $"{ApiBase}/housing/{housingIdSecondaryPlaceHolder}/image/{IdPlaceHolder}";

        /// <summary>
		/// The relative municipality list API endpoint address.
		/// </summary>
		private const string MunicipalityListApiEndpoint = $"{ApiBase}/municipalities";

        #endregion

        #region OtherConstants

        /// <summary>
        /// The broker API base address.
        /// </summary>
        private const string ApiBase = "broker-firm-api";      

		/// <summary>
		/// The primary ID placeholder used in API endpoint addresses.
		/// </summary>
		private const string IdPlaceHolder = "{id}";

        /// <summary>
        /// The housing ID placeholder used when the regular ID placeholder is already in use. 
        /// </summary>
        private const string housingIdSecondaryPlaceHolder = "{housingId}";

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

        /// <summary>
        /// Creates a new broker under the broker firm.
        /// </summary>
        /// <param name="brokerFirmId">The ID of the brokerfirm that the broker belongs to.</param>
        /// <param name="broker">The serialized DTO object to send.</param>
        /// <returns>A <see cref="Task"/>.</returns>
        /// <!-- Author: Marcus -->
        /// <!-- Co Authors: Jimmie -->
        public async Task<BrokerDto> CreateBroker([Required] int brokerFirmId, [Required] CreateBrokerDto broker)
        {
            List<KeyValuePair<string, string>> queries = new()
            {
                new KeyValuePair<string, string>("brokerFirmId", brokerFirmId.ToString()),
                new KeyValuePair<string, string>("returnCreatedBroker", true.ToString())
            };

            var response = await _httpClient.PostAsJsonAsync($"{BrokersApiEndPoint}/{BuildQueryString(queries)}", broker);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<BrokerDto>(new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            return EnsureNotNull(result, "Failed to create or serialize the resulting broker object.");
        }

        /// <summary>
        /// Deletes a broker.
        /// </summary>
        /// <param name="id">The ID of the broker object to delete.</param>
		/// <param name="brokerFirmId">The ID of the broker firm associated with the housing.</param>
        /// <returns>A <see cref="Task"/>.</returns>
        /// <!-- Author: Jimmie, Marcus -->
        /// <!-- Co Authors: -->
        public async Task DeleteBroker([Required] int id, [Required] int brokerFirmId)
        {
            var response = await _httpClient.DeleteAsync($"{BrokerByIdApiEndPoint.Replace(IdPlaceHolder, id.ToString())}{BuildQueryString("brokerFirmId", brokerFirmId.ToString())}");
            response.EnsureSuccessStatusCode();
        }

        /// <summary>
        /// Fetches data for a broker. 
        /// </summary>
        /// <param name="id">The ID of the broker to fetch.</param>
        /// <param name="brokerFirmId">The ID of the brokerfirm associated with the broker search.</param>
        /// <returns>A <see cref="Task"/> containing a <see cref="BrokerDto"/> object.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        public async Task<BrokerDto> GetBrokerById([Required] int id, [Required] int brokerFirmId)
        {
            var result = await _httpClient.GetFromJsonAsync<BrokerDto>($"{BrokerByIdApiEndPoint.Replace(IdPlaceHolder, id.ToString())}{BuildQueryString("brokerFirmId", brokerFirmId.ToString())}");
            return EnsureNotNull(result, "Failed to fetch or serialize the broker");
        }

        /// <summary>
        /// Fetches data for a broker. 
        /// </summary>
        /// <param name="brokerFirmId">The ID of the brokerfirm associated with the broker search.</param>
        /// <returns>A <see cref="Task"/> containing a <see cref="BrokerDto"/> object.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        public async Task<List<BrokerDto>> GetBrokers([Required] int brokerFirmId)
        {
            var result = await _httpClient.GetFromJsonAsync<List<BrokerDto>?>($"{BrokersApiEndPoint}{BuildQueryString("brokerFirmId", brokerFirmId.ToString())}");
            return EnsureNotNull(result, "Failed to fetch or serialize brokers.");
        }

        /// <summary>
        /// Updates a broker.
        /// </summary>
        /// <param name="id">The ID of the broker to update.</param>
        /// <param name="broker">The serialized DTO object to send.</param>
        /// <returns>A <see cref="Task"/>.</returns>
        /// <!-- Author: Jimmie, Marcus -->
        /// <!-- Co Authors: -->
        public async Task UpdateBroker([Required] int brokerId, [Required] EditBrokerDto broker)
        {
            var response = await _httpClient.PutAsJsonAsync($"{BrokerByIdApiEndPoint.Replace(IdPlaceHolder, brokerId.ToString())}{BuildQueryString("brokerFirmId", broker.BrokerFirmId.ToString())}", broker);
            response.EnsureSuccessStatusCode();
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
		public async Task<BrokerFirmDto> GetBrokerFirmById([Required] int brokerFirmId)
        {
            var result = await _httpClient.GetFromJsonAsync<BrokerFirmDto>(BrokerFirmByIdApiEndPoint.Replace(IdPlaceHolder, brokerFirmId.ToString()));
            return EnsureNotNull(result, "Failed to fetch or serialize the broker firm.");
        }

        /// <summary>
        /// Fetches statistics for a broker firm.
        /// </summary>
        /// <param name="brokerFirmId">The ID of the broker firm.</param>
        /// <returns>A <see cref="Task"/> containing a <see cref="BrokerFirmStatisticsDto"/> object.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        public async Task<BrokerFirmStatisticsDto> GetBrokerFirmStatistics([Required] int brokerFirmId)
        {
            var result = await _httpClient.GetFromJsonAsync<BrokerFirmStatisticsDto>(BrokerFirmStatisticsApiEndPoint.Replace(IdPlaceHolder, brokerFirmId.ToString()));
            return EnsureNotNull(result, "Failed to fetch or serialize the broker firm statistics object.");
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
        public async Task<HousingDto> CreateHousing([Required] int brokerFirmId, [Required] CreateHousingDto housing)
        {
            List<KeyValuePair<string, string>> queries = new()
            {
                new KeyValuePair<string, string>("brokerFirmId", brokerFirmId.ToString()),
                new KeyValuePair<string, string>("returnCreatedHousing", true.ToString())
            };

            var response = await _httpClient.PostAsJsonAsync($"{HousingApiEndPoint.Replace(IdPlaceHolder, housing.BrokerFirmId.ToString())}/{BuildQueryString(queries)}", housing);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<HousingDto>(new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            return EnsureNotNull(result, "Failed to serialize the returned housing object");
        }

        /// <summary>
        /// Deletes a housing object.
        /// </summary>
        /// <param name="brokerFirmId">The ID of the broker firm associated with the housing object.</param>
        /// <param name="housingId">The ID of the housing object to fetch images for.</param>
        /// <returns>A <see cref="Task"/>.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        public async Task DeleteHousing([Required] int brokerFirmId, int housingId)
        {
            var response = await _httpClient.DeleteAsync($"{HousingByIdApiEndPoint.Replace(IdPlaceHolder, housingId.ToString())}{BuildQueryString("brokerFirmId", brokerFirmId.ToString())}");
            response.EnsureSuccessStatusCode();
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
        public async Task<HousingDto> GetHousingById([Required] int id, [Required] int brokerFirmId)
        {
            var result = await _httpClient.GetFromJsonAsync<HousingDto>($"{HousingByIdApiEndPoint.Replace(IdPlaceHolder, id.ToString())}{BuildQueryString("brokerFirmId", brokerFirmId.ToString())}");
            return EnsureNotNull(result, "Failed to fetch or serialize the housing object");
        }

        /// <summary>
        /// Fetches all housing categories.
        /// </summary>
        /// <returns>A <see cref="Task"/> containing a collection of <see cref="HousingCategoryDto"/>.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        public async Task<List<HousingCategoryDto>> GetHousingCategories()
        {
            var result = await _httpClient.GetFromJsonAsync<List<HousingCategoryDto>>($"{HousingCategoryListApiEndpoint}");
            return EnsureNotNull(result, "Failed to fetch or serialize the housing categories");
        }

        /// <summary>
		/// Fetches housing count that is being handled by a broker.
		/// </summary>
		/// <param name="brokerId">The ID of the broker.</param>
		/// <returns>A <see cref="Task"/> containing a  <see cref="Int"/> Count</returns>
		public async Task<int> GetHousingCountByBrokerId(int brokerId)
        {
            return await _httpClient.GetFromJsonAsync<int>($"{HousingCountByBrokerApiEndpoint}{BuildQueryString("brokerId", brokerId.ToString())}");
        }

        /// <summary>
        /// Fetches all housing objects for a broker firm.
        /// </summary>
        /// <param name="brokerFirmId">The ID of the broker firm associated with the housing object.</param>
        /// <param name="limitImagesPerHousing">Sets the max limit of images to return per housing object.</param>
        /// <param name="brokerId">Filters the housing objects after a broker.</param>
        /// <returns>A <see cref="Task"/> containing a <see cref="HousingDto"/> object.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        public async Task<List<HousingDto>> GetHousings([Required] int brokerFirmId, int? limitImagesPerHousing = null, int? brokerId = null)
        {
            List<KeyValuePair<string, string>> queries = new()
            {
                new KeyValuePair<string, string>("brokerFirmId", brokerFirmId.ToString())
            };

            if (limitImagesPerHousing != null)
            {
                queries.Add(new KeyValuePair<string, string>("limitImagesPerHousing", limitImagesPerHousing.Value.ToString()));
            }

            if (brokerId != null)
            {
                queries.Add(new KeyValuePair<string, string>("brokerId", brokerId.ToString()!));
            }

            var result = await _httpClient.GetFromJsonAsync<List<HousingDto>?>($"{HousingApiEndPoint.Replace(IdPlaceHolder, brokerFirmId.ToString())}{BuildQueryString(queries)}");
            return EnsureNotNull(result, "Failed to fetch or serialize the housing objects");
        }

        /// <summary>
        /// Fetches all municipalities.
        /// </summary>
        /// <returns>A <see cref="Task"/> containing a collection of <see cref="MunicipalityDto"/>.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        public async Task<List<MunicipalityDto>> GetMunicipalities()
        {
            var result = await _httpClient.GetFromJsonAsync<List<MunicipalityDto>>(MunicipalityListApiEndpoint);
            return EnsureNotNull(result, "Failed to fetch or serialize the municipalties.");
        }

        /// <summary>
        /// Updates a housing object.
        /// </summary>
        /// <param name="housing">The serialized DTO object to send.</param>
        /// <returns>A <see cref="Task"/>.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        public async Task UpdateHousing([Required] EditHousingDto housing)
        {
            var response = await _httpClient.PutAsJsonAsync($"{HousingByIdApiEndPoint.Replace(IdPlaceHolder, housing.HousingId.ToString())}{BuildQueryString("brokerFirmId", housing.BrokerFirmId.ToString())}", housing);
            response.EnsureSuccessStatusCode();
        }        

        #endregion

        #region HousingImageMethods

        /// <summary>
        /// Fetches all images for a housing object. 
        /// </summary>
        /// <param name="brokerFirmId">The broker firm the housing object belongs to.</param>
        /// <param name="housingId">The ID of the housing object to fetch images for. </param>
        /// <returns>A <see cref="Task"/> containing a collection of <see cref="HousingDto"/> objects.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        public async Task<List<ImageDto>> GetHousingImages(int brokerFirmId, int housingId)
        {
            var result = await _httpClient.GetFromJsonAsync<List<ImageDto>>($"{HousingImageApiEndPoint.Replace(IdPlaceHolder, housingId.ToString())}{BuildQueryString("brokerFirmId", brokerFirmId.ToString())}");
            return EnsureNotNull(result, "Failed to fetch or serialize the housing images");
        }

        /// <summary>
        /// Deletes an image for a housing object.
        /// </summary>
        /// <param name="imageId">The ID of the image object to delete.</param>
		/// <param name="brokerFirmId">The ID of the broker firm associated with the housing object the image belongs to.</param>
        /// <param name="housingId">The ID of the housing object the image belongs to</param>
        /// <returns>A <see cref="Task"/>.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        public async Task DeleteHousingImage(int imageId, [Required] int brokerFirmId, [Required] int housingId)
        {
            string requestUrl = $"{HousingImageByIdApiEndPoint.Replace(housingIdSecondaryPlaceHolder, housingId.ToString()).Replace(IdPlaceHolder, imageId.ToString())}{BuildQueryString("brokerFirmId", brokerFirmId.ToString())}";
            var response = await _httpClient.DeleteAsync(requestUrl);
            response.EnsureSuccessStatusCode();
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
        public async Task DeleteImages([Required] int brokerFirmId, int housingId, List<int> imageIds)
        {
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Delete,
                $"{HousingImageApiEndPoint.Replace(IdPlaceHolder, housingId.ToString())}{BuildQueryString("brokerFirmId", brokerFirmId.ToString())}")
            {
                Content = JsonContent.Create(new DeleteImagesDto(housingId, imageIds))
            };

            var response = await _httpClient.SendAsync(requestMessage);
            response.EnsureSuccessStatusCode();
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
        public async Task<List<ImageDto>> UploadHousingImages([Required] int brokerFirmId, [Required] int housingId, List<IBrowserFile> newFiles)
        {
            if (newFiles.Count == 0)
            {
                throw new ArgumentException($"The collection '{newFiles}' can't be empty.", nameof(newFiles));
            }

            var content = new MultipartFormDataContent();

            foreach (var file in newFiles)
            {
                content.Add(new StreamContent(file.OpenReadStream()), "files", file.Name);
            }

            var response = await _httpClient.PostAsync($"{HousingImageApiEndPoint.Replace(IdPlaceHolder, housingId.ToString())}/{BuildQueryString("brokerFirmId", brokerFirmId.ToString())}", content);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<List<ImageDto>>(new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            return EnsureNotNull(result, "Failed to serialize the returned image objects");
        }

        #endregion

        #region BrokerProfileImageMethods

        /// <summary>
        /// Deletes an profileimage for a broker object.
        /// </summary>
        /// <param name="id">The ID of the image object to delete.</param>
		/// <param name="brokerFirmId">The ID of the broker firm associated with the broker object the profileimage belongs to.</param>
        /// <param name="brokerId">The ID of the broker object the image belongs to</param>
        /// <returns>A <see cref="Task"/>.</returns>
        /// <!-- Author: Marcus -->
        /// <!-- Co Authors: -->
        public Task DeleteBrokerProfileImage( [Required] int brokerFirmId, [Required] int brokerId)
        {
            return _httpClient.DeleteAsync($"{BrokerImageByIdApiEndPoint.Replace(IdPlaceHolder, brokerId.ToString())}{BuildQueryString("brokerFirmId", brokerFirmId.ToString())}");
        }

        /// <summary>
        /// Uploads profileimage for a broker object. 
        /// </summary>
        /// <param name="brokerFirmId">The ID of the broker firm associated with the broker object the profileimage belongs to.</param>
        /// <param name="brokerId">The ID of the broker object the image belongs to</param>
        /// <param name="newFile">The file to upload.</param>
        /// <returns>A <see cref="Task"/> containing a collection of <see cref="ImageDto"/> objects for the uploaded images.</returns>
        /// <!-- Author: Marcus -->
        /// <!-- Co Authors: Jimmie -->
        public async Task<ImageDto> UploadBrokerProfileImage([Required] int brokerFirmId, [Required] int brokerId, IBrowserFile newFile)
        {
            if (newFile == null)
            {
                throw new ArgumentException($"Cant find any image", nameof(newFile));
            }

            List<KeyValuePair<string, string>> queries = new()
            {
                new KeyValuePair<string, string>("brokerFirmId", brokerFirmId.ToString()),
                new KeyValuePair<string, string>("brokerId", brokerId.ToString())
            };

            var content = new MultipartFormDataContent();
            content.Add(new StreamContent(newFile.OpenReadStream()), "file", newFile.Name);

            var response = await _httpClient.PostAsync($"{BrokerProfileImageApiEndPoint}/{BuildQueryString(queries)}", content);            
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<ImageDto>(new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

            return EnsureNotNull(result, "Failed to serialize the returned images.");
        }

        #endregion
    }
}

using FribergFastigheter.Shared.Dto.Broker;
using FribergFastigheter.Shared.Dto.BrokerFirm;
using FribergFastigheter.Shared.Dto.Housing;
using FribergFastigheter.Shared.Dto.Image;
using FribergFastigheter.Shared.Dto.Login;
using FribergFastigheter.Shared.Dto.Statistics;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
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
        #region AdminApiConstants

        /// <summary>
        /// The broker API endpoint address.
        /// </summary>
        private const string AdminBrokerByIdApiEndPoint = $"{ApiBase}/admin/broker/{IdPlaceHolder}";

        #endregion

        #region BrokerApiConstants

        /// <summary>
        /// The broker API endpoint address.
        /// </summary>
        private const string BrokerByIdApiEndPoint = $"{ApiBase}/broker/{IdPlaceHolder}";

        /// <summary>
        /// The register broker API endpoint address.
        /// </summary>
        private const string BrokerCreationApiEndpoint = $"{ApiBase}/brokers/register";

        /// <summary>
        /// The broker login API endpoint address.
        /// </summary>
        private const string BrokerLoginApiEndPoint = $"{ApiBase}/brokers/login";

        // <summary>
        /// The broker profile image API endpoint address.
        /// </summary>
        private const string BrokerProfileImageApiEndPoint = $"{ApiBase}/broker/{IdPlaceHolder}/profile-image";

        /// <summary>
        /// The broker API endpoint address.
        /// </summary>
        private const string BrokersApiEndPoint = $"{ApiBase}/brokers";

        #endregion

        #region BrokerFirmApiConstants

        /// <summary>
        /// The broker firm API endpoint address.
        /// </summary>
        private const string BrokerFirmByIdApiEndPoint = $"{ApiBase}/firm";

        /// <summary>
		/// The broker firm API endpoint address.
		/// </summary>
		private const string BrokerFirmStatisticsApiEndPoint = $"{ApiBase}/firm/statistics";

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
		private const string HousingCategoryListApiEndpoint = $"{ApiBase}/housing/categories";

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
        /// The housing ID placeholder used when the regular ID placeholder is already in use. 
        /// </summary>
        private const string housingIdSecondaryPlaceHolder = "{housingId}";

        /// <summary>
        /// The primary ID placeholder used in API endpoint addresses.
        /// </summary>
        private const string IdPlaceHolder = "{id}";

        #endregion

        #region Fields

        /// <summary>
        /// The injected local autenthication state provider.
        /// </summary>
        protected readonly AuthenticationStateProvider _authenticationStateProvider;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="httpClient">The injected HTTP client.</param>
        /// <param name="authenticationStateProvider">The injected local autenthication state provider.</param>
        public BrokerFirmApiService(HttpClient httpClient, AuthenticationStateProvider authenticationStateProvider) : base(httpClient)
        {
            _authenticationStateProvider = authenticationStateProvider;
        }

        #endregion

        #region JwtMethods

        /// <summary>
        /// Sets the authorization header data for logged in brokers. 
        /// </summary>
        /// <returns>A <see cref="Task"/> representing an async operation.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        private async Task SetAuthorizationHeader()
        {
            var token = await ((ApiAuthenticationStateProvider)_authenticationStateProvider).GetTokenAsync();

            if (token != null)
            {
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
        }

        #endregion

        #region AdminMethods

        /// <summary>
        /// Performs an admin edit of a broker.
        /// </summary>
        /// <param name="id">The ID of the broker to update.</param>
        /// <param name="broker">The serialized DTO object to send.</param>
        /// <returns>A <see cref="Task"/> representing an async operation.</returns>
        /// <!-- Author: Jimmie  -->
        /// <!-- Co Authors: -->
        public async Task AdminEditBroker([Required] int brokerId, [Required] AdminEditBrokerDto broker)
        {
            await SetAuthorizationHeader();
            var response = await _httpClient.PutAsJsonAsync($"{AdminBrokerByIdApiEndPoint.Replace(IdPlaceHolder, brokerId.ToString())}", broker);
            response.EnsureSuccessStatusCode();
        }

        #endregion

        #region BrokerMethods

        /// <summary>
        /// Creates a new broker under the broker firm.
        /// </summary>
        /// <param name="broker">The serialized DTO object to send.</param>
        /// <returns>A <see cref="Task"/>.</returns>
        /// <!-- Author: Marcus -->
        /// <!-- Co Authors: Jimmie -->
        public async Task<CreatedBrokerDto> CreateBroker([Required] RegisterBrokerDto broker)
        {
            await SetAuthorizationHeader();
            var response = await _httpClient.PostAsJsonAsync($"{BrokerCreationApiEndpoint}", broker);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<CreatedBrokerDto>(new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            return EnsureNotNull(result, "Failed to create or serialize the resulting broker object.");
        }

        /// <summary>
        /// Deletes a broker.
        /// </summary>
        /// <param name="id">The ID of the broker object to delete.</param>
        /// <returns>A <see cref="Task"/>.</returns>
        /// <!-- Author: Jimmie  -->
        /// <!-- Co Authors: Marcus -->
        public async Task DeleteBroker([Required] int id)
        {
            await SetAuthorizationHeader();
            var response = await _httpClient.DeleteAsync($"{BrokerByIdApiEndPoint.Replace(IdPlaceHolder, id.ToString())}");
            response.EnsureSuccessStatusCode();
        }

        /// <summary>
        /// Performs a regular edit on the logged in broker. 
        /// </summary>
        /// <param name="id">The ID of the broker to update.</param>
        /// <param name="broker">The serialized DTO object to send.</param>
        /// <returns>A <see cref="Task"/>.</returns>
        /// <!-- Author: Jimmie  -->
        /// <!-- Co Authors: Marcus -->
        public async Task EditBroker([Required] int brokerId, [Required] EditBrokerDto broker)
        {
            await SetAuthorizationHeader();
            var response = await _httpClient.PutAsJsonAsync($"{BrokerByIdApiEndPoint.Replace(IdPlaceHolder, brokerId.ToString())}", broker);
            response.EnsureSuccessStatusCode();
        }

        /// <summary>
        /// Fetches data for a broker. 
        /// </summary>
        /// <param name="id">The ID of the broker to fetch.</param>
        /// <returns>A <see cref="Task"/> containing a <see cref="BrokerDto"/> object.</returns>
        /// <!-- Author: Jimmie  -->
        /// <!-- Co Authors: Marcus -->
        public async Task<BrokerDto> GetBrokerById([Required] int id)
        {
            await SetAuthorizationHeader();
            var result = await _httpClient.GetFromJsonAsync<BrokerDto>($"{BrokerByIdApiEndPoint.Replace(IdPlaceHolder, id.ToString())}");
            return EnsureNotNull(result, "Failed to fetch or serialize the broker");
        }

        /// <summary>
        /// Fetches data for a broker. 
        /// </summary>
        /// <returns>A <see cref="Task"/> containing a <see cref="BrokerDto"/> object.</returns>
        /// <!-- Author: Jimmie  -->
        /// <!-- Co Authors: Marcus -->
        public async Task<List<BrokerDto>> GetBrokers()
        {
            await SetAuthorizationHeader();
            var result = await _httpClient.GetFromJsonAsync<List<BrokerDto>?>($"{BrokersApiEndPoint}");
            return EnsureNotNull(result, "Failed to fetch or serialize brokers.");
        }

        /// <summary>
        /// Logs in a broker.
        /// </summary>
        /// <param name="loginData">The login data to submit.</param>
        /// <returns>A <see cref="Task"/> representing an async operation.</returns>
        /// <!-- Author: Jimmie  -->
        /// <!-- Co Authors: Marcus -->
        public async Task Login(LoginDto loginData)
        {
            var content = JsonContent.Create(loginData);

            var response = await _httpClient.PostAsync(BrokerLoginApiEndPoint, content);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<LoginResponseDto>(new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            EnsureNotNull(result, "Failed to serialize the response.");
            await ((ApiAuthenticationStateProvider)_authenticationStateProvider).SetTokenAsync(result!.Token);
        }

        #endregion

        #region BrokerFirmMethods

        /// <summary>
        /// Fetches a broker firm.
        /// </summary>
        /// <returns>A <see cref="Task"/> containing a <see cref="BrokerFirmDto"/> object.</returns>
        /// <!-- Author: Jimmie  -->
        /// <!-- Co Authors: Marcus -->
        public async Task<BrokerFirmDto> GetBrokerFirm()
        {
            await SetAuthorizationHeader();
            var result = await _httpClient.GetFromJsonAsync<BrokerFirmDto>(BrokerFirmByIdApiEndPoint);
            return EnsureNotNull(result, "Failed to fetch or serialize the broker firm.");
        }

        /// <summary>
        /// Fetches statistics for a broker firm.
        /// </summary>
        /// <returns>A <see cref="Task"/> containing a <see cref="BrokerFirmStatisticsDto"/> object.</returns>
        /// <!-- Author: Jimmie  -->
        /// <!-- Co Authors: Marcus -->
        public async Task<BrokerFirmStatisticsDto> GetBrokerFirmStatistics()
        {
            await SetAuthorizationHeader();
            var result = await _httpClient.GetFromJsonAsync<BrokerFirmStatisticsDto>(BrokerFirmStatisticsApiEndPoint);
            return EnsureNotNull(result, "Failed to fetch or serialize the broker firm statistics object.");
        }

        #endregion

        #region HousingMethods

        /// <summary>
        /// Creates a new housing under the broker firm.
        /// </summary>
        /// <param name="housing">The serialized DTO object to send.</param>
        /// <returns>A <see cref="Task"/> containg a <see cref="HousingDto"/> object if successful.</returns>
        /// <!-- Author: Jimmie  -->
        /// <!-- Co Authors: Marcus -->
        public async Task<HousingDto> CreateHousing([Required] CreateHousingDto housing)
        {
            await SetAuthorizationHeader();
            var response = await _httpClient.PostAsJsonAsync($"{HousingApiEndPoint}", housing);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<HousingDto>(new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            return EnsureNotNull(result, "Failed to serialize the returned housing object");
        }

        /// <summary>
        /// Deletes a housing object.
        /// </summary>
        /// <param name="housingId">The ID of the housing object to fetch images for.</param>
        /// <returns>A <see cref="Task"/>.</returns>
        /// <!-- Author: Jimmie  -->
        /// <!-- Co Authors: Marcus -->
        public async Task DeleteHousing(int housingId)
        {
            await SetAuthorizationHeader();
            var response = await _httpClient.DeleteAsync($"{HousingByIdApiEndPoint.Replace(IdPlaceHolder, housingId.ToString())}");
            response.EnsureSuccessStatusCode();
        }

        /// <summary>
        /// Fetches data for a housing object.
        /// </summary>
        /// <param name="id">The ID of the housing to fetch.</param>
        /// <returns>A <see cref="Task"/> containing a <see cref="HousingDto"/> object.</returns>
        /// <!-- Author: Jimmie  -->
        /// <!-- Co Authors: Marcus -->
        public async Task<HousingDto> GetHousingById([Required] int id)
        {
            await SetAuthorizationHeader();
            var result = await _httpClient.GetFromJsonAsync<HousingDto>($"{HousingByIdApiEndPoint.Replace(IdPlaceHolder, id.ToString())}");
            return EnsureNotNull(result, "Failed to fetch or serialize the housing object");
        }

        /// <summary>
        /// Fetches all housing categories.
        /// </summary>
        /// <returns>A <see cref="Task"/> containing a collection of <see cref="HousingCategoryDto"/>.</returns>
        /// <!-- Author: Jimmie  -->
        /// <!-- Co Authors: Marcus -->
        public async Task<List<HousingCategoryDto>> GetHousingCategories()
        {
            await SetAuthorizationHeader();
            var result = await _httpClient.GetFromJsonAsync<List<HousingCategoryDto>>($"{HousingCategoryListApiEndpoint}");
            return EnsureNotNull(result, "Failed to fetch or serialize the housing categories");
        }

        /// <summary>
		/// Fetches housing count that is being handled by a broker.
		/// </summary>
		/// <param name="brokerId">The ID of the broker.</param>
		/// <returns>A <see cref="Task"/> containing a  <see cref="Int"/> Count</returns>
        /// <!-- Author: Jimmie  -->
        /// <!-- Co Authors: Marcus -->
		public async Task<int> GetHousingCountByBrokerId(int brokerId)
        {
            await SetAuthorizationHeader();
            return await _httpClient.GetFromJsonAsync<int>($"{HousingCountByBrokerApiEndpoint}{BuildQueryString("brokerId", brokerId.ToString())}");
        }

        /// <summary>
        /// Fetches all housing objects for a broker firm.
        /// </summary>
        /// <param name="limitImagesPerHousing">Sets the max limit of images to return per housing object.</param>
        /// <param name="brokerId">Filters the housing objects after a broker.</param>
        /// <returns>A <see cref="Task"/> containing a <see cref="HousingDto"/> object.</returns>
        /// <!-- Author: Jimmie  -->
        /// <!-- Co Authors: Marcus -->
        public async Task<List<HousingDto>> GetHousings(int? limitImagesPerHousing = null, int? brokerId = null)
        {
            List<KeyValuePair<string, string>> queries = new();

            if (limitImagesPerHousing != null)
            {
                queries.Add(new KeyValuePair<string, string>("limitImagesPerHousing", limitImagesPerHousing.Value.ToString()));
            }

            if (brokerId != null)
            {
                queries.Add(new KeyValuePair<string, string>("brokerId", brokerId.ToString()!));
            }

            await SetAuthorizationHeader();
            var result = await _httpClient.GetFromJsonAsync<List<HousingDto>?>($"{HousingApiEndPoint}{BuildQueryString(queries)}");
            return EnsureNotNull(result, "Failed to fetch or serialize the housing objects");
        }

        /// <summary>
        /// Fetches all municipalities.
        /// </summary>
        /// <returns>A <see cref="Task"/> containing a collection of <see cref="MunicipalityDto"/>.</returns>
        /// <!-- Author: Jimmie  -->
        /// <!-- Co Authors: Marcus -->
        public async Task<List<MunicipalityDto>> GetMunicipalities()
        {
            await SetAuthorizationHeader();
            var result = await _httpClient.GetFromJsonAsync<List<MunicipalityDto>>(MunicipalityListApiEndpoint);
            return EnsureNotNull(result, "Failed to fetch or serialize the municipalties.");
        }

        /// <summary>
        /// Updates a housing object.
        /// </summary>
        /// <param name="housing">The serialized DTO object to send.</param>
        /// <returns>A <see cref="Task"/>.</returns>
        /// <!-- Author: Jimmie  -->
        /// <!-- Co Authors: Marcus -->
        public async Task UpdateHousing([Required] EditHousingDto housing)
        {
            await SetAuthorizationHeader();
            var response = await _httpClient.PutAsJsonAsync($"{HousingByIdApiEndPoint.Replace(IdPlaceHolder, housing.HousingId.ToString())}", housing);
            response.EnsureSuccessStatusCode();
        }

        #endregion

        #region HousingImageMethods

        /// <summary>
        /// Deletes an image for a housing object.
        /// </summary>
        /// <param name="imageId">The ID of the image object to delete.</param>
        /// <param name="housingId">The ID of the housing object the image belongs to</param>
        /// <returns>A <see cref="Task"/>.</returns>
        /// <!-- Author: Jimmie  -->
        /// <!-- Co Authors: Marcus -->
        public async Task DeleteHousingImage(int imageId, [Required] int housingId)
        {
            string requestUrl = $"{HousingImageByIdApiEndPoint.Replace(housingIdSecondaryPlaceHolder, housingId.ToString()).Replace(IdPlaceHolder, imageId.ToString())}";
            await SetAuthorizationHeader();
            var response = await _httpClient.DeleteAsync(requestUrl);
            response.EnsureSuccessStatusCode();
        }

        /// <summary>
        /// Deletes an image for a housing object.
        /// </summary>
        /// <param name="housingId">The ID of the housing object that owns the images. </param>
        /// <param name="imageIds">Contains the IDs of the images to delete.</param>
        /// <returns>A <see cref="Task"/>.</returns>
        /// <!-- Author: Jimmie  -->
        /// <!-- Co Authors: Marcus -->
        public async Task DeleteImages(int housingId, List<int> imageIds)
        {
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Delete,
                $"{HousingImageApiEndPoint.Replace(IdPlaceHolder, housingId.ToString())}")
            {
                Content = JsonContent.Create(new DeleteImagesDto(housingId, imageIds))
            };

            await SetAuthorizationHeader();
            var response = await _httpClient.SendAsync(requestMessage);
            response.EnsureSuccessStatusCode();
        }

        /// <summary>
        /// Fetches all images for a housing object. 
        /// </summary>
        /// <param name="housingId">The ID of the housing object to fetch images for. </param>
        /// <returns>A <see cref="Task"/> containing a collection of <see cref="HousingDto"/> objects.</returns>
        /// <!-- Author: Jimmie  -->
        /// <!-- Co Authors: Marcus -->
        public async Task<List<ImageDto>> GetHousingImages(int housingId)
        {
            await SetAuthorizationHeader();
            var result = await _httpClient.GetFromJsonAsync<List<ImageDto>>($"{HousingImageApiEndPoint.Replace(IdPlaceHolder, housingId.ToString())}");
            return EnsureNotNull(result, "Failed to fetch or serialize the housing images");
        }

        /// <summary>
        /// Uploads images for a housing object. 
        /// </summary>
        /// <param name="housingId">The ID of the housing object the image belongs to</param>
        /// <param name="newFiles">A collection of files to upload.</param>
        /// <returns>A <see cref="Task"/> containing a collection of <see cref="ImageDto"/> objects for the uploaded images.</returns>
        /// <!-- Author: Jimmie  -->
        /// <!-- Co Authors: Marcus -->
        public async Task<List<ImageDto>> UploadHousingImages([Required] int housingId, List<IBrowserFile> newFiles)
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

            await SetAuthorizationHeader();
            var response = await _httpClient.PostAsync($"{HousingImageApiEndPoint.Replace(IdPlaceHolder, housingId.ToString())}", content);
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
        /// <param name="brokerId">The ID of the broker object the image belongs to</param>
        /// <returns>A <see cref="Task"/>.</returns>
        /// <!-- Author: Marcus -->
        /// <!-- Co Authors: Jimmie -->
        public async Task DeleteBrokerProfileImage([Required] int brokerId)
        {
            await SetAuthorizationHeader();
            await _httpClient.DeleteAsync($"{BrokerProfileImageApiEndPoint.Replace(IdPlaceHolder, brokerId.ToString())}");
        }

        /// <summary>
        /// Uploads profileimage for a broker object. 
        /// </summary>
        /// <param name="brokerId">The ID of the broker object the image belongs to</param>
        /// <param name="newFile">The file to upload.</param>
        /// <returns>A <see cref="Task"/> containing a collection of <see cref="ImageDto"/> objects for the uploaded images.</returns>
        /// <!-- Author: Marcus, Jimmie -->
        /// <!-- Co Authors: -->
        public async Task<ImageDto> UploadBrokerProfileImage([Required] int brokerId, IBrowserFile newFile)
        {
            if (newFile == null)
            {
                throw new ArgumentException($"Cant find any image", nameof(newFile));
            }

            var content = new MultipartFormDataContent();
            content.Add(new StreamContent(newFile.OpenReadStream()), "file", newFile.Name);

            await SetAuthorizationHeader();
            var response = await _httpClient.PostAsync($"{BrokerProfileImageApiEndPoint.Replace(IdPlaceHolder, brokerId.ToString())}", content);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<ImageDto>(new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

            return EnsureNotNull(result, "Failed to serialize the returned images.");
        }

        #endregion
    }
}

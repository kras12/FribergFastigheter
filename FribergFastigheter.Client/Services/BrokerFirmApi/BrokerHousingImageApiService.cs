using FribergFastigheter.Shared.Dto;
using Microsoft.AspNetCore.Components.Forms;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;

namespace FribergFastigheter.Client.Services.HousingApi
{
    /// <summary>
    /// A service to fetch data from the Friberg Fastigheter Broker Housing Image API endpoint.
    /// </summary>
    /// <!-- Author: Jimmie -->
    /// <!-- Co Authors: -->
    public class BrokerHousingImageApiService : FribergApiServiceBase, IBrokerHousingImageApiService
    {
        #region Constants

        private const string ApiEndPoint = "api/BrokerFirm/Housing/Image";

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="httpClient">The injected HTTP client service.</param>
        public BrokerHousingImageApiService(FribergApiHttpClientService httpClientService) : base(httpClientService)
        {

        }

        #endregion

        #region Methods

        /// <summary>
        /// Uploads images for a housing object. 
        /// </summary>
        /// <param name="brokerFirmId">The ID of the broker firm associated with the housing object the image belongs to.</param>
        /// <param name="housingId">The ID of the housing object the image belongs to</param>
        /// <param name="newFiles">A collection of files to upload.</param>
        /// <returns>A <see cref="Task"/>.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        public Task UploadImages([Required] int brokerFirmId, [Required] int housingId, List<IBrowserFile> newFiles)
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
                content.Add(new StreamContent(file.OpenReadStream()), "Images", file.Name);
            }

            return _httpClient.PostAsync($"{ApiEndPoint}/{BuildQueryString(queries)}", content);            
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

            return _httpClient.DeleteAsync($"{ApiEndPoint}/{id}/{BuildQueryString(queries)}");
        }

        #endregion
    }
}

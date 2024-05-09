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
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace FribergFastigheter.Client.Services.FribergFastigheterApi
{
    /// <summary>
    /// An interface for the Friberg Fastigheter Broker Firm API.
    /// </summary>
    /// <!-- Author: Jimmie -->
    /// <!-- Co Authors: -->
    public interface IBrokerFirmApiService
    {
        #region BrokerMethods

        /// <summary>
        /// Creates a new broker under the broker firm.
        /// </summary>
        /// <param name="broker">The serialized DTO object to send.</param>
        /// <returns>A <see cref="Task"/>.</returns>
        /// <!-- Author: Marcus -->
        /// <!-- Co Authors: Jimmie -->
        public Task<CreatedBrokerDto> CreateBroker([Required] RegisterBrokerDto broker);

        /// <summary>
        /// Deletes a broker.
        /// </summary>
        /// <param name="id">The ID of the broker object to delete.</param>
        /// <returns>A <see cref="Task"/>.</returns>
        /// <!-- Author: Jimmie  -->
        /// <!-- Co Authors: Marcus -->
        public Task DeleteBroker([Required] int id);

        /// <summary>
        /// Fetches data for a broker. 
        /// </summary>
        /// <param name="id">The ID of the broker to fetch.</param>
        /// <returns>A <see cref="Task"/> containing a <see cref="BrokerDto"/> object.</returns>
        /// <!-- Author: Jimmie  -->
        /// <!-- Co Authors: Marcus -->
        public Task<BrokerDto> GetBrokerById([Required] int id);

        /// <summary>
        /// Fetches data for a broker. 
        /// </summary>
        /// <returns>A <see cref="Task"/> containing a <see cref="BrokerDto"/> object.</returns>
        /// <!-- Author: Jimmie  -->
        /// <!-- Co Authors: Marcus -->
        public Task<List<BrokerDto>> GetBrokers();

        /// <summary>
        /// Logs in a broker.
        /// </summary>
        /// <param name="loginData">The login data to submit.</param>
        /// <returns>A <see cref="Task"/> representing an async operation.</returns>
        /// <!-- Author: Jimmie  -->
        /// <!-- Co Authors: Marcus -->
        public Task Login(LoginDto loginData);

        /// <summary>
        /// Updates a broker.
        /// </summary>
        /// <param name="id">The ID of the broker to update.</param>
        /// <param name="broker">The serialized DTO object to send.</param>
        /// <returns>A <see cref="Task"/>.</returns>
        /// <!-- Author: Jimmie  -->
        /// <!-- Co Authors: Marcus -->
        public Task UpdateBroker([Required] int brokerId, [Required] EditBrokerDto broker);

        #endregion

        #region BrokerFirmMethods

        /// <summary>
        /// Fetches a broker firm.
        /// </summary>
        /// <returns>A <see cref="Task"/> containing a <see cref="BrokerFirmDto"/> object.</returns>
        /// <!-- Author: Jimmie  -->
        /// <!-- Co Authors: Marcus -->
        public Task<BrokerFirmDto> GetBrokerFirm();

        /// <summary>
        /// Fetches statistics for a broker firm.
        /// </summary>
        /// <returns>A <see cref="Task"/> containing a <see cref="BrokerFirmStatisticsDto"/> object.</returns>
        /// <!-- Author: Jimmie  -->
        /// <!-- Co Authors: Marcus -->
        public Task<BrokerFirmStatisticsDto> GetBrokerFirmStatistics();

        #endregion

        #region HousingMethods

        /// <summary>
        /// Creates a new housing under the broker firm.
        /// </summary>
        /// <param name="housing">The serialized DTO object to send.</param>
        /// <returns>A <see cref="Task"/> containg a <see cref="HousingDto"/> object if successful.</returns>
        /// <!-- Author: Jimmie  -->
        /// <!-- Co Authors: Marcus -->
        public Task<HousingDto> CreateHousing([Required] CreateHousingDto housing);

        /// <summary>
        /// Deletes a housing object.
        /// </summary>
        /// <param name="housingId">The ID of the housing object to fetch images for.</param>
        /// <returns>A <see cref="Task"/>.</returns>
        /// <!-- Author: Jimmie  -->
        /// <!-- Co Authors: Marcus -->
        public Task DeleteHousing(int housingId);

        /// <summary>
        /// Fetches data for a housing object.
        /// </summary>
        /// <param name="id">The ID of the housing to fetch.</param>
        /// <returns>A <see cref="Task"/> containing a <see cref="HousingDto"/> object.</returns>
        /// <!-- Author: Jimmie  -->
        /// <!-- Co Authors: Marcus -->
        public Task<HousingDto> GetHousingById([Required] int id);

        /// <summary>
        /// Fetches all housing categories.
        /// </summary>
        /// <returns>A <see cref="Task"/> containing a collection of <see cref="HousingCategoryDto"/>.</returns>
        /// <!-- Author: Jimmie  -->
        /// <!-- Co Authors: Marcus -->
        public Task<List<HousingCategoryDto>> GetHousingCategories();

        /// <summary>
		/// Fetches housing count that is being handled by a broker.
		/// </summary>
		/// <param name="brokerId">The ID of the broker.</param>
		/// <returns>A <see cref="Task"/> containing a  <see cref="Int"/> Count</returns>
        /// <!-- Author: Jimmie  -->
        /// <!-- Co Authors: Marcus -->
		public Task<int> GetHousingCountByBrokerId(int brokerId);

        /// <summary>
        /// Fetches all housing objects for a broker firm.
        /// </summary>
        /// <param name="limitImagesPerHousing">Sets the max limit of images to return per housing object.</param>
        /// <param name="brokerId">Filters the housing objects after a broker.</param>
        /// <returns>A <see cref="Task"/> containing a <see cref="HousingDto"/> object.</returns>
        /// <!-- Author: Jimmie  -->
        /// <!-- Co Authors: Marcus -->
        public Task<List<HousingDto>> GetHousings(int? limitImagesPerHousing = null, int? brokerId = null);

        /// <summary>
        /// Fetches all municipalities.
        /// </summary>
        /// <returns>A <see cref="Task"/> containing a collection of <see cref="MunicipalityDto"/>.</returns>
        /// <!-- Author: Jimmie  -->
        /// <!-- Co Authors: Marcus -->
        public Task<List<MunicipalityDto>> GetMunicipalities();

        /// <summary>
        /// Updates a housing object.
        /// </summary>
        /// <param name="housing">The serialized DTO object to send.</param>
        /// <returns>A <see cref="Task"/>.</returns>
        /// <!-- Author: Jimmie  -->
        /// <!-- Co Authors: Marcus -->
        public Task UpdateHousing([Required] EditHousingDto housing);

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
        public Task DeleteHousingImage(int imageId, [Required] int housingId);

        /// <summary>
        /// Deletes an image for a housing object.
        /// </summary>
        /// <param name="housingId">The ID of the housing object that owns the images. </param>
        /// <param name="imageIds">Contains the IDs of the images to delete.</param>
        /// <returns>A <see cref="Task"/>.</returns>
        /// <!-- Author: Jimmie  -->
        /// <!-- Co Authors: Marcus -->
        public Task DeleteImages(int housingId, List<int> imageIds);

        /// <summary>
        /// Fetches all images for a housing object. 
        /// </summary>
        /// <param name="housingId">The ID of the housing object to fetch images for. </param>
        /// <returns>A <see cref="Task"/> containing a collection of <see cref="HousingDto"/> objects.</returns>
        /// <!-- Author: Jimmie  -->
        /// <!-- Co Authors: Marcus -->
        public Task<List<ImageDto>> GetHousingImages(int housingId);

        /// <summary>
        /// Uploads images for a housing object. 
        /// </summary>
        /// <param name="housingId">The ID of the housing object the image belongs to</param>
        /// <param name="newFiles">A collection of files to upload.</param>
        /// <returns>A <see cref="Task"/> containing a collection of <see cref="ImageDto"/> objects for the uploaded images.</returns>
        /// <!-- Author: Jimmie  -->
        /// <!-- Co Authors: Marcus -->
        public Task<List<ImageDto>> UploadHousingImages([Required] int housingId, List<IBrowserFile> newFiles);

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
        public Task DeleteBrokerProfileImage([Required] int brokerId);

        /// <summary>
        /// Uploads profileimage for a broker object. 
        /// </summary>
        /// <param name="brokerId">The ID of the broker object the image belongs to</param>
        /// <param name="newFile">The file to upload.</param>
        /// <returns>A <see cref="Task"/> containing a collection of <see cref="ImageDto"/> objects for the uploaded images.</returns>
        /// <!-- Author: Marcus, Jimmie -->
        /// <!-- Co Authors: -->
        public Task<ImageDto> UploadBrokerProfileImage([Required] int brokerId, IBrowserFile newFile);

        #endregion
    }
}
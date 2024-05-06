using FribergFastigheter.Shared.Dto;
using FribergFastigheter.Shared.Dto.Statistics;
using Microsoft.AspNetCore.Components.Forms;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
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
        /// <param name="brokerFirmId">The ID of the brokerfirm that the broker belongs to.</param>
        /// <param name="broker">The serialized DTO object to send.</param>
        /// <returns>A <see cref="Task"/>.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        public Task<BrokerDto> CreateBroker([Required] int brokerFirmId, [Required] RegisterBrokerDto broker);

        /// <summary>
        /// Deletes a broker.
        /// </summary>
        /// <param name="id">The ID of the broker object to delete.</param>
		/// <param name="brokerFirmId">The ID of the broker firm associated with the housing.</param>
        /// <returns>A <see cref="Task"/>.</returns>
        /// <!-- Author: Jimmie, Marcus -->
        /// <!-- Co Authors: -->
        public Task DeleteBroker([Required] int id, [Required] int brokerFirmId);

        /// <summary>
        /// Fetches data for a broker. 
        /// </summary>
        /// <param name="id">The ID of the broker to fetch.</param>
        /// <param name="brokerFirmId">The ID of the brokerfirm associated with the broker search.</param>
        /// <returns>A <see cref="Task"/> containing a <see cref="BrokerDto"/> object.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        public Task<BrokerDto> GetBrokerById([Required] int id, [Required] int brokerFirmId);

        /// <summary>
        /// Fetches data for a broker. 
        /// </summary>
        /// <param name="brokerFirmId">The ID of the brokerfirm associated with the broker search.</param>
        /// <returns>A <see cref="Task"/> containing a <see cref="BrokerDto"/> object.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        public Task<List<BrokerDto>> GetBrokers([Required] int brokerFirmId);

        /// <summary>
        /// Updates a broker.
        /// </summary>
        /// <param name="id">The ID of the broker to update.</param>
        /// <param name="broker">The serialized DTO object to send.</param>
        /// <returns>A <see cref="Task"/>.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        public Task UpdateBroker([Required] int brokerId, [Required] EditBrokerDto broker);

		#endregion

		#region BrokerFirmMethods

		/// <summary>
		/// Fetches a broker firm.
		/// </summary>
		/// <param name="brokerFirmId">The ID of the brokerfirm.</param>
		/// <returns>A <see cref="Task"/> containing a <see cref="BrokerFirmDto"/> object.</returns>
		/// <!-- Author: Jimmie -->
		/// <!-- Co Authors: -->
		public Task<BrokerFirmDto> GetBrokerFirmById([Required] int brokerFirmId);

        /// <summary>
        /// Fetches statistics for a broker firm.
        /// </summary>
        /// <param name="brokerFirmId">The ID of the broker firm.</param>
        /// <returns>A <see cref="Task"/> containing a <see cref="BrokerFirmStatisticsDto"/> object.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        public Task<BrokerFirmStatisticsDto> GetBrokerFirmStatistics([Required] int brokerFirmId);

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
        public Task<HousingDto> CreateHousing([Required] int brokerFirmId, [Required] CreateHousingDto housing);

        /// <summary>
        /// Deletes a housing object.
        /// </summary>
        /// <param name="brokerFirmId">The ID of the broker firm associated with the housing object.</param>
        /// <param name="housingId">The ID of the housing object to fetch images for.</param>
        /// <returns>A <see cref="Task"/>.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        public Task DeleteHousing([Required] int brokerFirmId, int housingId);

        /// <summary>
        /// Fetches data for a housing object.
        /// </summary>
        /// <param name="id">The ID of the housing to fetch.</param>
        /// <param name="brokerFirmId">The ID of the broker firm associated with the housing object.</param>
        /// <returns>A <see cref="Task"/> containing a <see cref="HousingDto"/> object.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        /// <param name="includeImageData"></param>
        public Task<HousingDto> GetHousingById([Required] int id, [Required] int brokerFirmId);

        /// <summary>
        /// Fetches all housing categories.
        /// </summary>
        /// <returns>A <see cref="Task"/> containing a collection of <see cref="HousingCategoryDto"/>.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        public Task<List<HousingCategoryDto>> GetHousingCategories();

        /// <summary>
		/// Fetches housing count that is being handled by a broker.
		/// </summary>
		/// <param name="brokerId">The ID of the broker.</param>
		/// <returns>A <see cref="Task"/> containing a  <see cref="Int"/> Count</returns>
		public Task<int> GetHousingCountByBrokerId(int brokerId);

        /// <summary>
        /// Fetches all housing objects for a broker firm.
        /// </summary>
        /// <param name="brokerFirmId">The ID of the broker firm associated with the housing object.</param>
        /// <param name="limitImagesPerHousing">Sets the max limit of images to return per housing object.</param>
        /// <param name="brokerId">Filters the housing objects after a broker.</param>
        /// <returns>A <see cref="Task"/> containing a <see cref="HousingDto"/> object.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        public Task<List<HousingDto>> GetHousings([Required] int brokerFirmId, int? limitImagesPerHousing = null, int? brokerId = null);

        /// <summary>
        /// Fetches all municipalities.
        /// </summary>
        /// <returns>A <see cref="Task"/> containing a collection of <see cref="MunicipalityDto"/>.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        public Task<List<MunicipalityDto>> GetMunicipalities();

        /// <summary>
        /// Updates a housing object.
        /// </summary>
        /// <param name="housing">The serialized DTO object to send.</param> 
        /// <returns>A <see cref="Task"/>.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        public Task UpdateHousing([Required] EditHousingDto housing);

        #endregion

        #region HousingImageMethods

        /// <summary>
        /// Fetches all images for a housing object. 
        /// </summary>
        /// <param name="brokerFirmId">The broker firm the housing object belongs to.</param>
        /// <param name="housingId">The ID of the housing object to fetch images for. </param>
        /// <returns>A <see cref="Task"/> containing a collection of <see cref="HousingDto"/> objects.</returns>
        public Task<List<ImageDto>> GetHousingImages(int brokerFirmId, int housingId);

        /// <summary>
        /// Deletes an image for a housing object.
        /// </summary>
        /// <param name="id">The ID of the image object to delete.</param>
        /// <param name="brokerFirmId">The ID of the broker firm associated with the housing object the image belongs to.</param>
        /// <param name="housingId">The ID of the housing object the image belongs to</param>
        /// <returns>A <see cref="Task"/>.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        public Task DeleteHousingImage(int id, [Required] int brokerFirmId, [Required] int housingId);

        /// <summary>
        /// Deletes images for a housing object.
        /// </summary>
        /// <param name="brokerFirmId">The ID of the broker firm associated with the housing object that owns the images.</param>
        /// <param name="housingId">The ID of the housing object that owns the images. </param>
        /// <param name="imageIds">Contains the IDs of the images to delete.</param>
        /// <returns>A <see cref="Task"/>.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        public Task DeleteImages([Required] int brokerFirmId, int housingId, List<int> imageIds);

        /// <summary>
        /// Uploads images for a housing object. 
        /// </summary>
        /// <param name="brokerFirmId">The ID of the broker firm associated with the housing object the image belongs to.</param>
        /// <param name="housingId">The ID of the housing object the image belongs to</param>
        /// <param name="newFiles">A collection of files to upload.</param>
        /// <returns>A <see cref="Task"/> containing a collection of <see cref="ImageDto"/> objects for the uploaded images.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        public Task<List<ImageDto>> UploadHousingImages([Required] int brokerFirmId, [Required] int housingId, List<IBrowserFile> newFiles);

        #endregion

        #region BrokerImageMethods

        /// <summary>
        /// Deletes an profileimage for a broker object.
        /// </summary>
        /// <param name="id">The ID of the image object to delete.</param>
		/// <param name="brokerFirmId">The ID of the broker firm associated with the broker object the profileimage belongs to.</param>
        /// <param name="brokerId">The ID of the broker object the image belongs to</param>
        /// <returns>A <see cref="Task"/>.</returns>
        /// <!-- Author: Marcus -->
        /// <!-- Co Authors: -->
        public Task DeleteBrokerProfileImage([Required] int brokerFirmId, [Required] int brokerId);

        /// <summary>
        /// Uploads profileimage for a broker object. 
        /// </summary>
        /// <param name="brokerFirmId">The ID of the broker firm associated with the broker object the profileimage belongs to.</param>
        /// <param name="brokerId">The ID of the broker object the image belongs to</param>
        /// <param name="newFile">The file to upload.</param>
        /// <returns>A <see cref="Task"/> containing a collection of <see cref="ImageDto"/> objects for the uploaded images.</returns>
        /// <!-- Author: Marcus -->
        /// <!-- Co Authors: Jimmie -->
        public Task<ImageDto> UploadBrokerProfileImage([Required] int brokerFirmId, [Required] int brokerId, IBrowserFile newFile);    

        #endregion
    }
}
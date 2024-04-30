using FribergFastigheter.Shared.Dto;
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
        public Task CreateBroker([Required] int brokerFirmId, [Required] CreateBrokerDto broker);

        /// <summary>
        /// Deletes a broker.
        /// </summary>
        /// <param name="id">The ID of the broker object to delete.</param>
        /// <param name="brokerFirmId">The ID of the broker firm associated with the housing.</param>
        /// <returns>A <see cref="Task"/>.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        public Task DeleteBroker(int id, [Required] int brokerFirmId);

        /// <summary>
        /// Fetches data for a broker. 
        /// </summary>
        /// <param name="id">The ID of the broker to fetch.</param>
        /// <param name="brokerFirmId">The ID of the brokerfirm associated with the broker search.</param>
        /// <returns>A <see cref="Task"/> containing a <see cref="BrokerDto"/> object.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        public Task<BrokerDto?> GetBrokerById([Required] int id, [Required] int brokerFirmId);

        /// <summary>
        /// Fetches data for a broker. 
        /// </summary>
        /// <param name="brokerFirmId">The ID of the brokerfirm associated with the broker search.</param>
        /// <returns>A <see cref="Task"/> containing a <see cref="BrokerDto"/> object.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        public Task<List<BrokerDto>?> GetBrokers([Required] int brokerFirmId);

        /// <summary>
        /// Updates a broker.
        /// </summary>
        /// <param name="id">The ID of the broker to update.</param>
        /// <param name="broker">The serialized DTO object to send.</param>
        /// <returns>A <see cref="Task"/>.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        public Task UpdateBroker([Required] int id, [Required] UpdateBrokerDto broker);

		#endregion

		#region BrokerFirmMethods

		/// <summary>
		/// Fetches a broker firm.
		/// </summary>
		/// <param name="brokerFirmId">The ID of the brokerfirm.</param>
		/// <returns>A <see cref="Task"/> containing a <see cref="BrokerFirmDto"/> object.</returns>
		/// <!-- Author: Jimmie -->
		/// <!-- Co Authors: -->
		public Task<BrokerFirmDto?> GetBrokerFirmById([Required] int brokerFirmId);

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
        public Task<HousingDto?> CreateHousing([Required] int brokerFirmId, [Required] CreateHousingDto housing);

        /// <summary>
        /// Deletes a housing object.
        /// </summary>
        /// <param name="id">The ID of the housing object to delete.</param>
        /// <param name="brokerFirmId">The ID of the broker firm associated with the housing object.</param>
        /// <returns>A <see cref="Task"/>.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        public Task DeleteHousing(int id, [Required] int brokerFirmId);

        /// <summary>
        /// Fetches data for a housing object.
        /// </summary>
        /// <param name="id">The ID of the housing to fetch.</param>
        /// <param name="brokerFirmId">The ID of the broker firm associated with the housing object.</param>
        /// <returns>A <see cref="Task"/> containing a <see cref="HousingDto"/> object.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        /// <param name="includeImageData"></param>
        public Task<HousingDto?> GetHousingById([Required] int id, [Required] int brokerFirmId);

        /// <summary>
        /// Fetches all housing categories.
        /// </summary>
        /// <returns>A <see cref="Task"/> containing a collection of <see cref="HousingCategoryDto"/>.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        public Task<List<HousingCategoryDto>?> GetHousingCategories();

        /// <summary>
        /// Fetches data for housing objects. 
        /// </summary>
        /// <param name="brokerFirmId">The ID of the brokerfirm associated with the housing objects.</param>
        /// <returns>A <see cref="Task"/> containing a <see cref="HousingDto"/> object.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        public Task<List<HousingDto>?> GetHousings([Required] int brokerFirmId, int? limitImagesPerHousing = null);

        /// <summary>
        /// Fetches all municipalities.
        /// </summary>
        /// <returns>A <see cref="Task"/> containing a collection of <see cref="MunicipalityDto"/>.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        public Task<List<MunicipalityDto>?> GetMunicipalities();

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
        /// Deletes an image for a housing object.
        /// </summary>
        /// <param name="id">The ID of the image object to delete.</param>
        /// <param name="brokerFirmId">The ID of the broker firm associated with the housing object the image belongs to.</param>
        /// <param name="housingId">The ID of the housing object the image belongs to</param>
        /// <returns>A <see cref="Task"/>.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        public Task DeleteImage(int id, [Required] int brokerFirmId, [Required] int housingId);

        /// <summary>
        /// Uploads images for a housing object. 
        /// </summary>
        /// <param name="brokerFirmId">The ID of the broker firm associated with the housing object the image belongs to.</param>
        /// <param name="housingId">The ID of the housing object the image belongs to</param>
        /// <param name="newFiles">A collection of files to upload.</param>
        /// <returns>A <see cref="Task"/>.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        public Task UploadImages([Required] int brokerFirmId, [Required] int housingId, List<IBrowserFile> newFiles);
        Task<List<HousingDto>?> GetHousingsByBrokerId(int brokerId, int brokerFirmId, int? limitImagesPerHousing = null);

        #endregion
    }
}
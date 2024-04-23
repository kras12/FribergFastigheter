using FribergFastigheter.Shared.Dto;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Net;

namespace FribergFastigheter.Client.Services.HousingApi
{
    /// <summary>
    /// An interface for the Friberg Fastigheter Broker Housing API endpoint.
    /// </summary>
    /// <!-- Author: Jimmie -->
    /// <!-- Co Authors: -->
    internal interface IBrokerHousingApiService
    {
        /// <summary>
        /// Creates a new housing under the broker firm.
        /// </summary>
        /// <param name="brokerFirmId">The ID of the brokerfirm that the housing belongs to.</param>
        /// <param name="housing">The serialized DTO object to send.</param>
        /// <returns>A <see cref="Task"/>.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        public Task CreateHousing([Required] int brokerFirmId, [Required] CreateHousingDto housing);

        /// <summary>
        /// Deletes a housing object.
        /// </summary>
        /// <param name="id">The ID of the housing object to delete.</param>
		/// <param name="brokerFirmId">The ID of the broker firm associated with the housing object.</param>
        /// <returns>A <see cref="Task"/>.</returns>
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
        /// Fetches data for housing objects. 
        /// </summary>
        /// <param name="brokerFirmId">The ID of the brokerfirm associated with the housing objects.</param>
        /// <returns>A <see cref="Task"/> containing a <see cref="HousingDto"/> object.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        public Task<HousingDto?> GetHousings([Required] int brokerFirmId);

        /// <summary>
        /// Updates a housing object.
        /// </summary>
        /// <param name="id">The ID of the housing object to update.</param>
        /// <param name="brokerFirmId">The ID of the brokerfirm associated with the housing objects.</param>
        /// <param name="housing">The serialized DTO object to send.</param>
        /// <returns>A <see cref="Task"/>.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        public Task UpdateHousing([Required] int id, [Required] int brokerFirmId, [Required] UpdateHousingDto housing);
    }
}
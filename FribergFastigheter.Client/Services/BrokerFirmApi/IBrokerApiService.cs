using FribergFastigheter.Shared.Dto;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Net;

namespace FribergFastigheter.Client.Services.HousingApi
{
    // <summary>
    /// An interface for a Friberg Fastigheter Broker API endpoint.
    /// </summary>
    /// <!-- Author: Jimmie -->
    /// <!-- Co Authors: -->
    public interface IBrokerApiService
    {
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
        /// <param name="id">The ID of the housing object to delete.</param>
		/// <param name="brokerFirmId">The ID of the broker firm associated with the housing.</param>
        /// <returns></returns>
        public Task DeleteBroker(int id, [Required] int brokerFirmId);

        /// <summary>
        /// Fetches data for a broker. 
        /// </summary>
        /// <param name="id">The ID of the broker to fetch.</param>
        /// <param name="brokerFirmId">The ID of the brokerfirm associated with the broker search.</param>
        /// <returns>A <see cref="Task"/> containing a <see cref="BrokerDto"/> object.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        /// <param name="includeImageData"></param>
        public Task<BrokerDto?> GetBrokerById([Required] int id, [Required] int brokerFirmId);

        /// <summary>
        /// Fetches data for a broker. 
        /// </summary>
        /// <param name="brokerFirmId">The ID of the brokerfirm associated with the broker search.</param>
        /// <returns>A <see cref="Task"/> containing a <see cref="BrokerDto"/> object.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        public Task<List<BrokerDto?>> GetBrokers([Required] int brokerFirmId);

        /// <summary>
        /// Updates a broker.
        /// </summary>
        /// <param name="brokerFirmId">The ID of the brokerfirm that the broker belongs to.</param>
        /// <param name="broker">The serialized DTO object to send.</param>
        /// <returns>A <see cref="Task"/>.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        public Task UpdateBroker([Required] int brokerFirmId, [Required] UpdateBrokerDto broker);
    }
}
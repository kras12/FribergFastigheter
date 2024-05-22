using FribergFastigheter.Shared.Dto.Statistics;
using FribergFastigheter.Server.Data.Entities;
using FribergFastigheterApi.Data.DatabaseContexts;

namespace FribergFastigheter.Server.Data.Interfaces
{
    /// <summary>
    /// Interface for BrokerFirmRepository
    /// </summary>
    /// <!-- Author: Marcus -->
    /// <!-- Co Authors: Jimmie -->
    public interface IBrokerFirmRepository
    {
        /// <summary>
        /// Adds a broker firm.
        /// </summary>
        /// <param name="brokerFirm">The broker firm.</param>
        /// <returns>A <see cref="Task"/> representing an async operation.</returns>
        public Task AddAsync(BrokerFirm brokerFirm);

        /// <summary>
        /// Adds a collection of broker firms.
        /// </summary>
        /// <param name="brokerFirms">The broker firms to add.</param>
        /// <returns>A <see cref="Task"/> representing an async operation.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        public Task AddAsync(List<BrokerFirm> brokerFirms);

        /// <summary>
        /// Gets the broker count for a broker firm.
        /// </summary>
        /// <param name="brokerFirmId">The ID of the broker firm.</param>
        /// <returns>The number of brokers in the broker firm.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors:  -->
        public Task<int> BrokerCount(int brokerFirmId);

        /// <summary>
        /// Deletes a broker firm.
        /// </summary>
        /// <param name="brokerFirm">The broker firm.</param>
        /// <returns>A <see cref="Task"/> representing an async operation.</returns>
        public Task DeleteAsync(BrokerFirm brokerFirm);

        /// <summary>
        /// Checks if a broker firm exists.
        /// </summary>
        /// <param name="brokerFirmId">The ID of the broker firm.</param>
        /// <returns>True if the broker firm exists.</returns>
        public Task<bool> Exists(int brokerFirmId);

        /// <summary>
        /// Gets all broker firms.
        /// </summary>
        /// <returns>A collection of broker firms that were found.</returns>
        public Task<List<BrokerFirm>> GetAllBrokerFirmsAsync();

        /// <summary>
        /// Gets a broker firm by ID.
        /// </summary>
        /// <param name="id">The ID of the broker firm.</param>
        /// <param name="includeDeletedBrokers">True to include deleted brokers.</param>
        /// <returns>A broker firm if found.</returns>
        public Task<BrokerFirm?> GetBrokerFirmByIdAsync(int id, bool? includeDeletedBrokers = true);

        /// <summary>
        /// Returns statistics for a broker firm. 
        /// </summary>
        /// <param name="brokerFirmId">The ID of the broker firm to fetch statistics for.</param>
        /// <returns>A <see cref="Task"/> containining a <see cref="BrokerFirmStatisticsDto"/> object with the statistics.</returns>
        /// <exception cref="Exception"></exception>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        public Task<BrokerFirmStatisticsDto?> GetStatistics(int brokerFirmId);

        /// <summary>
        /// Checks if a broker firm have a specific broker.
        /// </summary>
        /// <param name="brokerFirmId">The ID of the broker firm.</param>
        /// <param name="brokerId">The ID of the broker.</param>
        /// <returns>True if the broker belongs to the firm.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors:  -->
        public Task<bool> HaveBroker(int brokerFirmId, int brokerId);

        /// <summary>
        /// Updates a broker firm.
        /// </summary>
        /// <param name="brokerFirm">The broker firm.</param>
        /// <returns>A <see cref="Task"/> representing an async operation.</returns>
        public Task UpdateAsync(BrokerFirm brokerFirm);
    }
}

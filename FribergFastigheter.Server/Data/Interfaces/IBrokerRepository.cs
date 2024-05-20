using FribergFastigheter.Server.Data.Entities;
using FribergFastigheterApi.Data.DatabaseContexts;
using Microsoft.EntityFrameworkCore;

namespace FribergFastigheter.Server.Data.Interfaces
{
    /// <summary>
	/// Interface for BrokerRepository
	/// </summary>
	/// <!-- Author: Marcus -->
	/// <!-- Co Authors: -->
    /// 
    public interface IBrokerRepository
    {
        /// <summary>
        /// Adds a broker.
        /// </summary>
        /// <param name="broker">The broker to add.</param>
        /// <returns>A <see cref="Task"/> representing an async operation.</returns>
        public Task AddAsync(Broker broker);

        /// <summary>
        /// Adds a profile image to the broker.
        /// </summary>
        /// <param name="brokerId">The ID of the broker.</param>
        /// <param name="image">The image to add.</param>
        /// <returns>A <see cref="Task"/> representing an async operation.</returns>
        /// <exception cref="Exception"></exception>
        /// <!-- Author: Jimmie, Marcus -->
        /// <!-- Co Authors: -->
        public Task AddImage(int brokerId, Image image);

        /// <summary>
        /// Deletes a broker.
        /// </summary>
        /// <param name="brokerId">The ID of the broker.</param>
        /// <returns>A <see cref="Task"/> representing an async operation.</returns>
        /// <exception cref="Exception"></exception>
        /// <!-- Author: Marcus, Jimmie -->
        /// <!-- Co Authors: -->
        public Task DeleteAsync(int brokerId);

        /// <summary>
        /// Deletes the profile image of a broker.
        /// </summary>
        /// <param name="brokerId">The ID of the broker.</param>
        /// <returns>A <see cref="Task"/> representing an async operation.</returns>
        /// <exception cref="Exception"></exception>
        /// <!-- Author: Jimmie, Marcus -->
        /// <!-- Co Authors: -->
        public Task DeleteProfileImage(int brokerId);

        /// <summary>
        /// Checks whether a broker exists.
        /// </summary>
        /// <param name="id">The ID of the broker.</param>
        /// <returns>True if the broker exists.</returns>
        /// <!-- Author: Jimmie, Marcus -->
        /// <!-- Co Authors: -->
        public Task<bool> Exists(int id);

        /// <summary>
        /// Gets a broker by ID.
        /// </summary>
        /// <param name="id">The ID of the broker.</param>
        /// <returns>The broker if found.</returns>
        public Task<Broker?> GetBrokerByIdAsync(int id);

        /// <summary>
        /// Gets a broker by the user ID.
        /// </summary>
        /// <param name="id">The user ID of the broker.</param>
        /// <returns>The broker if found.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        public Task<Broker?> GetBrokerByUserIdAsync(string id);

        /// <summary>
        /// Gets all brokers that matches the criterias.
        /// </summary>
        /// <param name="brokerFirmId">Sets the broker firm ID as a filter.</param>
        /// <param name="includeDeleted">True to include deleted brokers.</param>
        /// <returns>A collection of the brokers found.</returns>
        /// <!-- Author: Marcus, Jimmie -->
        /// <!-- Co Authors: -->
        public Task<List<Broker>> GetBrokersAsync(int? brokerFirmId = null, bool includeDeleted = false);

        /// <summary>
        /// Gets all brokers with an aggregated housing count.
        /// </summary>
        /// <param name="brokerFirmId">Sets the broker firm ID as a filter.</param>
        /// <param name="includeDeleted">True to include deleted brokers.</param>
        /// <returns>A collection of the brokers found.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        public Task<List<BrokerWithHousingCount>> GetBrokersWithHousingCountAsync(int? brokerFirmId = null, bool includeDeleted = false);

        /// <summary>
        /// Gets the profile image of the broker.
        /// </summary>
        /// <param name="brokerId">The ID of the broker.</param>
        /// <returns>The profile image if found.</returns>
        /// <!-- Author: Jimmie, Marcus -->
        /// <!-- Co Authors: -->
        public Task<Image?> GetProfileImage(int brokerId);

        /// <summary>
        /// Updates a broker and its user entity.
        /// </summary>
        /// <param name="broker"></param>
        /// <returns>A <see cref="Task"/> representing an async operation.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        public Task UpdateAsync(Broker broker);
    }
}

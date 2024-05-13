using FribergFastigheter.Server.Data.Entities;

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
        Task AddAsync(Broker broker);
        Task AddImage(int brokerId, Image imageId);
        Task DeleteAsync(int brokerId);
        Task DeleteProfileImage(int brokerId);
        Task<bool> Exists(int id);
        Task<List<Broker>> GetBrokersAsync(int? brokerFirmId = null, bool includeDeleted = false);
        Task<Broker?> GetBrokerByIdAsync(int id);
        public Task<Broker?> GetBrokerByUserIdAsync(string id);
        Task<Image?> GetProfileImage(int brokerId);
        Task UpdateAsync(Broker broker);
    }
}

using FribergFastigheterApi.Data.Entities;

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
        Task<List<Broker>> GetAllBrokersAsync();
        Task<List<Broker>> GetAllBrokersByBrokerFirmIdAsync(int brokerFirmId);
        Task<Broker?> GetBrokerByIdAsync(int id);
        Task<Image?> GetProfileImage(int brokerId);
        Task<bool> IsOwnedByBrokerFirm(int id, int BrokerFirmId);
        Task<bool> OwnsImage(int brokerId, int imageId);
        Task UpdateAsync(Broker broker);
    }
}

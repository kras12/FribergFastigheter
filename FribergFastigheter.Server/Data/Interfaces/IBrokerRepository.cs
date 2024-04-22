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
        Task<bool> BelongsToBrokerFirm(int brokerId, int brokerFirmId);
        Task DeleteAsync(int brokerId);
        Task DeleteAsync(Broker broker);
        Task<List<Broker>> GetAllBrokersAsync();
        Task<List<Broker>> GetAllBrokersByBrokerFirmIdAsync(int brokerFirmId);
        Task<Broker?> GetBrokerByIdAsync(int id);
        Task UpdateAsync(Broker broker);
    }
}

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
        Task DeleteAsync(Broker broker);
        Task<List<Broker>> GetAllBrokersAsync();
        Task<Broker?> GetBrokerByIdAsync(int id);
        Task UpdateAsync(Broker broker);
    }
}

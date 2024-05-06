using FribergFastigheter.Shared.Dto.Statistics;
using FribergFastigheterApi.Data.Entities;

namespace FribergFastigheter.Server.Data.Interfaces
{
    /// <summary>
    /// Interface for BrokerFirmRepository
    /// </summary>
    /// <!-- Author: Marcus -->
    /// <!-- Co Authors: Jimmie -->
    /// 

    public interface IBrokerFirmRepository
    {
        Task AddAsync(BrokerFirm brokerFirm);
        Task DeleteAsync(BrokerFirm brokerFirm);
        Task<List<BrokerFirm>> GetAllBrokerFirmsAsync();
        Task<BrokerFirm?> GetBrokerFirmByIdAsync(int id);
        Task UpdateAsync(BrokerFirm brokerFirm);
        Task<int> BrokerCount(int brokerFirmId);
		Task<bool> HaveBroker(int brokerFirmId, int brokerId);
        Task<bool> Exists(int brokerFirmId);
        Task<BrokerFirmStatisticsDto> GetStatistics(int brokerFirmId);
    }
}

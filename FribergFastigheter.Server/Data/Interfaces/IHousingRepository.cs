using FribergFastigheter.Data.Entities;

namespace FribergFastigheter.Server.Data.Interfaces
{
	/// <summary>
	/// Interface for HousingRepository
	/// </summary>
	/// <!-- Author: Marcus -->
	/// <!-- Co Authors: Jimmie -->
	public interface IHousingRepository
    {
        Task AddAsync(Housing housing);
		Task DeleteAsync(int housingId);
		Task DeleteAsync(Housing housing);
        Task<List<Housing>> GetAllHousingAsync(int? municipalityId = null, int? brokerId = null, int? brokerFirm = null);
        Task<Housing?> GetHousingByIdAsync(int id);
        Task UpdateAsync(Housing housing);
		Task<bool> IsOwnedByBrokerFirm(int id, int BrokerFirmId);
		Task<bool> Exists(int id);
    }
}

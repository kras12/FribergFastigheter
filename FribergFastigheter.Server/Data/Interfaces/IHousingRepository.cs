using FribergFastigheter.Data.Entities;

namespace FribergFastigheter.Server.Data.Interfaces
{
    /// <summary>
	/// Interface for HousingRepository
	/// </summary>
	/// <!-- Author: Marcus -->
	/// <!-- Co Authors: -->
    /// 

    public interface IHousingRepository
    {
        Task AddAsync(Housing housing);
        Task DeleteAsync(Housing housing);
        Task<List<Housing>> GetAllHousingAsync();
        Task<Housing?> GetHousingByIdAsync(int id);
        Task UpdateAsync(Housing housing);
    }
}

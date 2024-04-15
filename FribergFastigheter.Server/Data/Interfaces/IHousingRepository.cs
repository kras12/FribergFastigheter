using FribergFastigheter.Data.Entities;

namespace FribergFastigheter.Server.Data.Interfaces
{
	/// <summary>
	/// Interface for HousingRepository
	/// </summary>
	/// <!-- Author: Marcus -->
	/// <!-- Co Authors: -->
	/// <!-- Author: Marcus -->
	/// <!-- Co Authors: Jimmie -->
	public interface IHousingRepository
    {
        Task AddAsync(Housing housing);
        Task DeleteAsync(Housing housing);
        Task<List<Housing>> GetAllHousingAsync(int? housingCategoryId);
        Task<Housing?> GetHousingByIdAsync(int id);
        Task UpdateAsync(Housing housing);
    }
}

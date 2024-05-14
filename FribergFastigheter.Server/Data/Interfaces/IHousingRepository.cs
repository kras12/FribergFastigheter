using FribergFastigheter.Server.Data.Entities;

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
        Task<List<Housing>> GetHousingsAsync(int? brokerId = null, int? brokerFirmId = null, int? municipalityId = null,
            int? housingCategoryId = null, int? limitHousings = null, int? limitImagesPerHousing = null, decimal? minPrice = null, decimal? maxPrice = null,
            double? minLivingArea = null, double? maxLivingArea = null, int? offsetRows = null, bool includeDeleted = false);
        Task UpdateAsync(Housing housing);
		Task<bool> Exists(int id);
		Task<List<Image>> GetImages(int housingId, List<int>? imageIds = null);
		Task<bool> HousingExists(int housingId);
        Task<int> DeleteImages(int housingId, List<int>? imageIds = null);
        Task<int> DeleteImage(int housingId, int imageId);
        Task AddImages(int housingId, List<Image> imageIds);
        Task<List<HousingCategory>> GetHousingCategories();
        Task<List<Municipality>> GetMunicipalities();
        Task<int> GetHousingsCountAsync(int? brokerId = null, int? brokerFirm = null, int? municipalityId = null, int? housingCategoryId = null, decimal? minPrice = null, decimal? maxPrice = null, double? minLivingArea = null, double? maxLivingArea = null);
		Task<Housing?> GetHousingByIdAsync(int housingId, int? brokerFirmId = null);
        Task AddAsync(List<Housing> housings);
    }
}

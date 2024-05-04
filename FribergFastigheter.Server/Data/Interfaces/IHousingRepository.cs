using FribergFastigheter.Data.Entities;
using FribergFastigheterApi.Data.Entities;

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
		Task<Broker> GetBroker(int housingId);
		Task<List<Housing>> GetHousingsAsync(int? brokerId = null, int? brokerFirmId = null, int? municipalityId = null, int? housingCategoryId = null, int? limitHousings = null, int? limitImagesPerHousing = null, decimal? minPrice = null, decimal? maxPrice = null, double? minLivingArea = null, double? maxLivingArea = null, int? offsetRows = null);
        Task UpdateAsync(Housing housing);
		Task<bool> IsOwnedByBrokerFirm(int id, int BrokerFirmId);
		Task<bool> Exists(int id);
		Task<List<Image>> GetImages(int housingId, List<int>? imageIds = null);
		Task<bool> HousingExists(int housingId);
        Task<bool> OwnsImage(int housingId, int imageId);
        Task<int> DeleteImages(int housingId, List<int> imageIds);
        Task<int> DeleteImage(int housingId, int imageId);
        Task<Image?> GetImagebyId(int housingId, int imageId);
        Task AddImages(int housingId, List<Image> imageIds);
        Task<List<HousingCategory>> GetHousingCategories();
        Task<List<Municipality>> GetMunicipalities();
        Task<int> GetHousingsCountAsync(int? brokerId = null, int? brokerFirm = null, int? municipalityId = null, int? housingCategoryId = null, decimal? minPrice = null, decimal? maxPrice = null, double? minLivingArea = null, double? maxLivingArea = null);
		Task<bool> OwnsImages(int housingId, List<int> imageIds);
        Task<Housing?> GetHousingByIdAsync(int housingId, int? brokerFirmId = null);
    }
}

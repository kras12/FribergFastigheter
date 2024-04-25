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
        Task<List<Housing>> GetAllHousingAsync(int? municipalityId = null, int? brokerId = null, int? brokerFirm = null, int? limitHousings = null, int? limitImagesPerHousing = null);
        Task<Housing?> GetHousingByIdAsync(int id);
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
    }
}

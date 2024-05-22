using FribergFastigheter.Server.Data.Entities;
using FribergFastigheterApi.Data.DatabaseContexts;
using Microsoft.EntityFrameworkCore;

namespace FribergFastigheter.Server.Data.Interfaces
{
	/// <summary>
	/// Interface for HousingRepository
	/// </summary>
	/// <!-- Author: Marcus -->
	/// <!-- Co Authors: Jimmie -->
	public interface IHousingRepository
    {
        /// <summary>
        /// Adds a housing object.
        /// </summary>
        /// <param name="housing">The housing object to add. </param>
        /// <returns>A <see cref="Task"/> representing an async operation.</returns>
        public Task AddAsync(Housing housing);

        /// <summary>
        /// Adds a collection of housing objects.
        /// </summary>
        /// <param name="housings">The housing objects to add.</param>
        /// <returns>A <see cref="Task"/> representing an async operation.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        public Task AddAsync(List<Housing> housings);

        /// <summary>
        /// Add images to a housing object. 
        /// </summary>
        /// <param name="housingId">The ID of the housing object.</param>
        /// <param name="images">The images to add.</param>
        /// <returns>A <see cref="Task"/> representing an async operation.</returns>
        /// <exception cref="Exception"></exception>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        public Task AddImages(int housingId, List<Image> images);

        /// <summary>
        /// Deletes a housing object.
        /// </summary>
        /// <param name="housingId">The ID of the housing object.</param>
        /// <returns>A <see cref="Task"/> representing an async operation.</returns>
        public Task DeleteAsync(int housingId);

        /// <summary>
        /// Deletes a housing object.
        /// </summary>
        /// <param name="housing">The housing object.</param>
        /// <returns>A <see cref="Task"/> representing an async operation.</returns>
		public Task DeleteAsync(Housing housing);

        /// <summary>
        /// Deletes an image for a housing object.
        /// </summary>
        /// <param name="housingId">The ID of the housing object.</param>
        /// <param name="imageId">The ID of the image.</param>
        /// <returns>A <see cref="Task"/> containing the number of images deleted.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
		public Task<int> DeleteImage(int housingId, int imageId);

        /// <summary>
        /// Deletes images for a housing object.
        /// </summary>
        /// <param name="housingId">The ID of the housing object.</param>
        /// <param name="imageIds">A collection of IDs for the images to delete.</param>
        /// <returns>A <see cref="Task"/> containing the number of images deleted.</returns>
        /// <exception cref="Exception"></exception>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        public Task<int> DeleteImages(int housingId, List<int>? imageIds = null);

        /// <summary>
        /// Checks if a housing object exists.
        /// </summary>
        /// <param name="id">The ID of the housing object.</param>
        /// <returns>A <see cref="Task"/> containing the value true if the housing object exists.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        public Task<bool> Exists(int id);

        /// <summary>
        /// Get a housing object by ID.
        /// </summary>
        /// <param name="housingId">The ID of the housing object.</param>
        /// <param name="brokerFirmId">The ID of the broker firm associated with the housing object.</param>
        /// <returns></returns>
        /// <!-- Author: Marcus, Jimmie -->
        /// <!-- Co Authors: -->
        public Task<Housing?> GetHousingByIdAsync(int housingId, int? brokerFirmId = null);

        /// <summary>
        /// Returns all housing categories.
        /// </summary>
        /// <returns>A collection of housing categories.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
		public Task<List<HousingCategory>> GetHousingCategories();

        /// <summary>
        /// Gets a collection of housing objects.
        /// </summary>
        /// <param name="brokerId">The ID of the broker associated with the housing object.</param>
        /// <param name="brokerFirmId">The ID of the broker firm associated with the housing object.</param>
        /// <param name="municipalityId">The ID of the municipality to filter by.</param>
        /// <param name="housingCategoryId">The ID of the housing category to filter by.</param>
        /// <param name="limitHousings">Sets a limit of the number of housing objects to retrieve.</param>
        /// <param name="limitImagesPerHousing">Sets a limit of the number of images per housing object to retrieve.</param>
        /// <param name="minPrice">Sets a filter of the minimum price of a housing object.</param>
        /// <param name="maxPrice">Sets a filter of the maxmimum price of a housing object.</param>
        /// <param name="minLivingArea">Sets a filter of the minimum living area of a housing object.</param>
        /// <param name="maxLivingArea">Sets a filter of the maximum living area of a housing object.</param>
        /// <param name="offsetRows">Offset the rows to fetch.</param>
        /// <param name="includeDeleted">True to include deleted housing objects.</param>
        /// <returns>A collection of housing objects that match the criterias.</returns>
        /// <!-- Author: Marcus, Jimmie -->
        /// <!-- Co Authors: -->
        public Task<List<Housing>> GetHousingsAsync(int? brokerId = null, int? brokerFirmId = null, int? municipalityId = null,
            int? housingCategoryId = null, int? limitHousings = null, int? limitImagesPerHousing = null, decimal? minPrice = null, decimal? maxPrice = null,
            double? minLivingArea = null, double? maxLivingArea = null, int? offsetRows = null, bool includeDeleted = false);

        /// <summary>
        /// Gets the number of housing objects that matchs certain criterias.
        /// </summary>
        /// <param name="brokerId">The ID of the broker associated with the housing object.</param>
        /// <param name="brokerFirmId">The ID of the broker firm associated with the housing object.</param>
        /// <param name="municipalityId">The ID of the municipality to filter by.</param>
        /// <param name="housingCategoryId">The ID of the housing category to filter by.</param>
        /// <param name="minPrice">Sets a filter of the minimum price of a housing object.</param>
        /// <param name="maxPrice">Sets a filter of the maxmimum price of a housing object.</param>
        /// <param name="minLivingArea">Sets a filter of the minimum living area of a housing object.</param>
        /// <param name="maxLivingArea">Sets a filter of the maximum living area of a housing object.</param>
        /// <returns>A collection of housing objects that match the criterias.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        public Task<int> GetHousingsCountAsync(int? brokerId = null, int? brokerFirmId = null, int? municipalityId = null,
            int? housingCategoryId = null, decimal? minPrice = null, decimal? maxPrice = null, double? minLivingArea = null, double? maxLivingArea = null);

        /// <summary>
        /// Gets images that belongs to a housing object.
        /// </summary>
        /// <param name="housingId">The ID of the housing object.</param>
        /// <param name="imageIds">A collection of image IDs to filter by.</param>
        /// <returns>A collection of images.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        public Task<List<Image>> GetImages(int housingId, List<int>? imageIds = null);

        /// <summary>
        /// Gets all municipalities.
        /// </summary>
        /// <returns>A collection of municipalities</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        public Task<List<Municipality>> GetMunicipalities();

        /// <summary>
        /// Checks Whether a housing object exists. 
        /// </summary>
        /// <param name="housingId">The ID of the housing object.</param>
        /// <returns>True if the housing object exists.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        public Task<bool> HousingExists(int housingId);

        /// <summary>
        /// Updates a housing object.
        /// </summary>
        /// <param name="housing">The housing object to update.</param>
        /// <returns></returns>
        /// <!-- Author: Marcus, Jimmie -->
        /// <!-- Co Authors: -->
        /// <returns>A <see cref="Task"/> representing an async operation.</returns>
        public Task UpdateAsync(Housing housing);        
    }
}

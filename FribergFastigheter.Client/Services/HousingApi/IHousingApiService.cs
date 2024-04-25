using FribergFastigheter.Shared.Dto;

namespace FribergFastigheter.Client.Services.HousingApi
{
    /// <summary>
    /// An interface for a service to fetch data from Friberg Fastigheter Housing API endpoints.
    /// </summary>
    /// <!-- Author: Jimmie -->
    /// <!-- Co Authors: -->
    public interface IHousingApiService
    {
        /// <summary>
        /// Fetches all housings that matches the filters and options.
        /// </summary>
        /// <param name="limitHousings">Sets the max limit of housing objects to return.</param>
        /// <param name="limitImageCountPerHousing">Sets the max limit of images to return per housing object.</param>
        /// <param name="municipalityId">Sets a filter on municipality.</param>
        /// <param name="housingCategoryId"></param>
        /// <param name="minPrice">An optional min price filter.</param>
        /// <param name="maxPrice">An optional max price filter.</param>
        /// <param name="minLivingArea">An optional min living area filter.</param>
        /// <returns>A <see cref="Task"/> containing a collection of <see cref="HousingDto"/>.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        /// <param name="maxLivingArea">An optional max living area filter.</param>
        public Task<List<HousingDto>?> SearchHousings(int? limitHousings = null, int? limitImageCountPerHousing = null, int? municipalityId = null,
            int? housingCategoryId = null, decimal? minPrice = null,
            decimal? maxPrice = null, double? minLivingArea = null, double? maxLivingArea = null);
    }
}
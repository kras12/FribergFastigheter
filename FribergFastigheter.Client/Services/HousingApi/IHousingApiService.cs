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
        /// <param name="municipalityId">Sets a filter on municipality.</param>
        /// <param name="limitHousings">Sets the max limit of housing objects to return.</param>
        /// <param name="limitImageCountPerHousing">Sets the max limit of images to return per housing object.</param>
        /// <returns>A <see cref="Task"/> containing a collection of <see cref="HousingDto"/>.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        Task<List<HousingDto>?> SearchHousings(int? municipalityId = null, int? limitHousings = null, int? limitImageCountPerHousing = null);
    }
}
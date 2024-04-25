using FribergFastigheter.Shared.Dto;

namespace FribergFastigheter.Client.Services.HousingApi
{
    /// <summary>
    /// An interface for a service to fetch data from the Friberg Fastigheter Housing Category API endpoint.
    /// </summary>
    /// <!-- Author: Jimmie -->
    /// <!-- Co Authors: -->
    internal interface IHousingCategoryApiService
    {
        /// <summary>
        /// Fetches all housing categories.
        /// </summary>
        /// <returns>A <see cref="Task"/> containing a collection of <see cref="HousingCategoryDto"/>.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        public Task<List<HousingCategoryDto>?> GetCategories();
    }
}
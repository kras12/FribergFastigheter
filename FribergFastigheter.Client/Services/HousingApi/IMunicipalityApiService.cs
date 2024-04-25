using FribergFastigheter.Shared.Dto;

namespace FribergFastigheter.Client.Services.HousingApi
{
    /// <summary>
    /// An interface for a service to fetch data from the Friberg Fastigheter Municipality API endpoint.
    /// </summary>
    /// <!-- Author: Jimmie -->
    /// <!-- Co Authors: -->
    internal interface IMunicipalityApiService
    {
        /// <summary>
        /// Fetches all municipalities.
        /// </summary>
        /// <returns>A <see cref="Task"/> containing a collection of <see cref="MunicipalityDto"/>.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        public Task<List<MunicipalityDto>?> GetMunicipalities();
    }
}
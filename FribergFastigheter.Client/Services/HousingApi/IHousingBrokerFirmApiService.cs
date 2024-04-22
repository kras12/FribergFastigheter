using FribergFastigheter.Shared.Dto;

namespace FribergFastigheter.Client.Services.HousingApi
{
    /// <summary>
    /// Interface class for a service to fetch data from the Friberg Fastigheter Housing Broker Firm API endpoint.
    /// </summary>
    /// <!-- Author: Jimmie -->
    /// <!-- Co Authors: -->
    public interface IHousingBrokerFirmApiService
    {
        /// <summary>
        /// Fetches data for a broker firm. 
        /// </summary>
        /// <param name="id">The ID of the broker firm.</param>
        /// <returns>A <see cref="Task"/> containing a collection of <see cref="BrokerFirmDto"/> objects.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        /// <param name="includeImageData"></param>
        Task<BrokerFirmDto?> GetBrokerFirmById(int id);
    }
}
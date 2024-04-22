using FribergFastigheter.Shared.Dto;

namespace FribergFastigheter.Client.Services.HousingApi
{
    /// <summary>
    /// Interface class for a service to fetch data from the Friberg Fastigheter Housing Broker API endpoint.
    /// </summary>
    /// <!-- Author: Jimmie -->
    /// <!-- Co Authors: -->
    public interface IHousingBrokerApiService
    {
        /// <summary>
        /// Fetches data for a broker. 
        /// </summary>
        /// <param name="id">The ID of the broker.</param>
        /// <returns>A <see cref="Task"/> containing a collection of <see cref="BrokerDto"/> objects.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        /// <param name="includeImageData"></param>
        Task<BrokerDto?> GetBrokerById(int id);
    }
}
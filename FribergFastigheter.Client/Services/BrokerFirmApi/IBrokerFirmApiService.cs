using FribergFastigheter.Shared.Dto;
using System.ComponentModel.DataAnnotations;

namespace FribergFastigheter.Client.Services.HousingApi
{
    /// <summary>
    /// A an interface for the Friberg Fastigheter Broker Firm API endpoint.
    /// </summary>
    /// <!-- Author: Jimmie -->
    /// <!-- Co Authors: -->
    internal interface IBrokerFirmApiService
    {
        /// <summary>
        /// Fetches a broker firm.
        /// </summary>
        /// <param name="id">The ID of the brokerfirm.</param>
        /// <returns>A <see cref="Task"/> containing a <see cref="BrokerFirmDto"/> object.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        /// <param name="includeImageData"></param>
        public Task<BrokerFirmDto?> GetBrokerFirmById([Required] int id);
    }
}
using FribergFastigheter.Shared.Dto.Broker;
using FribergFastigheter.Shared.Dto.BrokerFirm;
using FribergFastigheter.Shared.Dto.Housing;

namespace FribergFastigheter.Client.Services.FribergFastigheterApi
{
    /// <summary>
    /// An interface for a service to fetch data from Friberg Fastigheter Housing API endpoints.
    /// </summary>
    /// <!-- Author: Jimmie -->
    /// <!-- Co Authors: -->
    public interface IHousingApiService
    {
        /// <summary>
        /// Fetches a broker by ID.
        /// </summary>
        /// <param name="id">The ID of the broker.</param>
        /// <returns>A <see cref="Task"/> containing a collection of <see cref="BrokerDto"/> objects.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        /// <param name="includeImageData"></param>
        Task<BrokerDto> GetBrokerById(int id);

        /// <summary>
        /// Fetches a broker firm by ID.
        /// </summary>
        /// <param name="id">The ID of the broker firm.</param>
        /// <returns>A <see cref="Task"/> containing a <see cref="BrokerFirmDto"/> object.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        public Task<BrokerFirmDto> GetBrokerFirmById(int id);

		public Task<List<BrokerDto>> GetBrokers(int brokerFirmId);

		/// <summary>
		/// Fetches a housing object by ID.
		/// </summary>
		/// <param name="housingId">The ID of the housing object.</param>
		/// <returns>A <see cref="Task"/> containing a <see cref="HousingDto"/> object.</returns>
		public Task<HousingDto> GetHousingById(int housingId);

		/// <summary>
		/// Fetches all housing categories.
		/// </summary>
		/// <returns>A <see cref="Task"/> containing a collection of <see cref="HousingCategoryDto"/>.</returns>
		/// <!-- Author: Jimmie -->
		/// <!-- Co Authors: -->
		public Task<List<HousingCategoryDto>> GetHousingCategories();
		Task<List<HousingDto>> GetHousings(int brokerFirmId, int? limitImagesPerHousing = null, int? brokerId = null);

		/// <summary>
		/// Fetches housing objects that are assignt to a specific broker.
		/// </summary>
		/// <param name="brokerId">The ID of the broker.</param>
		/// <param name="limitImagesPerHousing">Sets the max limit of images to return per housing object.</param>
		/// <returns>A <see cref="Task"/> containing a collection of <see cref="HousingDto"/> objects if successful.</returns>
		public Task<List<HousingDto>> GetHousingsByBrokerId(int brokerId, int? limitImagesPerHousing = null);

		/// <summary>
		/// Fetches all municipalities.
		/// </summary>
		/// <returns>A <see cref="Task"/> containing a collection of <see cref="MunicipalityDto"/>.</returns>
		/// <!-- Author: Jimmie -->
		/// <!-- Co Authors: -->
		public Task<List<MunicipalityDto>> GetMunicipalities();

        /// <summary>
        /// Fetches all housings that matches the filters and options.
        /// </summary>
        /// <param name="maxNumberOfResultsPerPage">Sets the max limit of housing objects to return per page. Set a number to enable pagination.</param>
        /// <param name="limitImageCountPerHousing">Sets the max limit of images to return per housing object.</param>
        /// <param name="municipalityId">Sets a filter on municipality.</param>
        /// <param name="housingCategoryId"></param>
        /// <param name="minPrice">An optional min price filter.</param>
        /// <param name="maxPrice">An optional max price filter.</param>
        /// <param name="minLivingArea">An optional min living area filter.</param>
        /// <param name="maxLivingArea">An optional max living area filter.</param>
        /// <param name="offsetRows">An optional number of rows to skip.</param>
        /// <returns>A <see cref="Task"/> containing a <see cref="HousingSearchResultDto"/> object if successful.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        public Task<HousingSearchResultDto> SearchHousings(int? maxNumberOfResultsPerPage = null, int? limitImageCountPerHousing = null,
            int? municipalityId = null, int? housingCategoryId = null, decimal? minPrice = null,
            decimal? maxPrice = null, double? minLivingArea = null, double? maxLivingArea = null, int? offsetRows = null);
    }
}
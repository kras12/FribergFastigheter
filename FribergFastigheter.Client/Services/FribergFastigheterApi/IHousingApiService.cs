﻿using FribergFastigheter.Shared.Dto;

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
		/// Fetches data for a broker associated with a housing object.
		/// </summary>
		/// <param name="id">The ID of the housing object.</param>
		/// <returns>A <see cref="Task"/> containing a <see cref="BrokerDto"/> object if successful.</returns>
		/// <!-- Author: Jimmie -->
		/// <!-- Co Authors: -->
		public Task<BrokerDto?> GetBrokerByHousingId(int id);

		/// <summary>
		/// Fetches data for a broker. 
		/// </summary>
		/// <param name="id">The ID of the broker.</param>
		/// <returns>A <see cref="Task"/> containing a collection of <see cref="BrokerDto"/> objects.</returns>
		/// <!-- Author: Jimmie -->
		/// <!-- Co Authors: -->
		/// <param name="includeImageData"></param>
		Task<BrokerDto?> GetBrokerById(int id);

		/// <summary>
		/// Fetches data for a broker firm. 
		/// </summary>
		/// <param name="id">The ID of the broker firm.</param>
		/// <returns>A <see cref="Task"/> containing a <see cref="BrokerFirmDto"/> object.</returns>
		/// <!-- Author: Jimmie -->
		/// <!-- Co Authors: -->
		public Task<BrokerFirmDto?> GetBrokerFirmById(int id);

		/// <summary>
		/// Fetches data for a broker firm. 
		/// </summary>
		/// <param name="id">The ID of the broker firm.</param>
		/// <returns>A <see cref="Task"/> containing a collection of <see cref="BrokerFirmDto"/> objects.</returns>
		/// <!-- Author: Jimmie -->
		/// <!-- Co Authors: -->
		/// <summary>
		/// Fetches a housing object by ID.
		/// </summary>
		/// <param name="housingId">The ID of the housing object.</param>
		/// <returns>A <see cref="Task"/> containing a <see cref="HousingDto"/> object.</returns>
		public Task<HousingDto?> GetHousingById(int housingId);

		/// <summary>
		/// Fetches all housing categories.
		/// </summary>
		/// <returns>A <see cref="Task"/> containing a collection of <see cref="HousingCategoryDto"/>.</returns>
		/// <!-- Author: Jimmie -->
		/// <!-- Co Authors: -->
		public Task<List<HousingCategoryDto>?> GetHousingCategories();

        /// <summary>
        /// Fetches housing objects that is being handled by a broker.
        /// </summary>
        /// <param name="brokerId">The ID of the broker.</param>
        /// <param name="limitImagesPerHousing">Sets the max limit of images to return per housing object.</param>
        /// <returns>A <see cref="Task"/> containing a collection of <see cref="HousingDto"/> objects if successful.</returns>
        public Task<List<HousingDto>?> GetHousingsByBrokerId(int brokerId, int? limitImagesPerHousing = null);

		/// <summary>
		/// Fetches all municipalities.
		/// </summary>
		/// <returns>A <see cref="Task"/> containing a collection of <see cref="MunicipalityDto"/>.</returns>
		/// <!-- Author: Jimmie -->
		/// <!-- Co Authors: -->
		public Task<List<MunicipalityDto>?> GetMunicipalities();

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
        public Task<HousingSearchResultDto?> SearchHousings(int? maxNumberOfResultsPerPage = null, int? limitImageCountPerHousing = null,
            int? municipalityId = null, int? housingCategoryId = null, decimal? minPrice = null,
            decimal? maxPrice = null, double? minLivingArea = null, double? maxLivingArea = null, int? offsetRows = null);
    }
}
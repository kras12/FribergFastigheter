using AutoMapper;
using FribergFastigheter.Client.Models.Housing;
using FribergFastigheter.Client.Services.FribergFastigheterApi;
using Microsoft.AspNetCore.Components;

namespace FribergFastigheter.Client.Services
{
    public class HousingSearchService
    {
        #region Fields

        /// <summary>
        /// Contains the input used for the last housing search.
        /// </summary>
        private HousingSearchInputViewModel _lastHousingSearchInput { get; set; } = new();

        

        /// <summary>
        /// The injected auto mapper.
        /// </summary>
        private IMapper _autoMapper { get; set; }

        /// <summary>
        /// The injected service for the housing API.
        /// </summary>
        private IHousingApiService _housingApi { get; set; }

        #endregion

        #region InjectedServiceProperties
#pragma warning disable CS8618



#pragma warning restore CS8618
        #endregion

        #region OtherProperties

        public Task InitalizeTask { get; private set; }

        /// <summary>
        /// True if the user have performed a housing search.
        /// </summary>
        public bool HaveSearchedHousings { get; set; } = false;

        /// <summary>
        /// The result of the housing search.
        /// </summary>
        public HousingSearchResultViewModel? HousingSearchResult { get; set; } = null;

        /// <summary>
        /// Binds the input data from the housing search form.
        /// </summary>
        [SupplyParameterFromForm]
        public HousingSearchInputViewModel HousingSearchFormInput { get; set; } = new();

        #endregion

        #region Constructors

        public HousingSearchService(IMapper autoMapper, IHousingApiService housingApi)
        {
            _autoMapper = autoMapper;
            _housingApi = housingApi;
            InitalizeTask = Task.WhenAll(LoadHousingCategories(), LoadMunicipalities());
        }

        #endregion

        #region Methods

        /// <summary>
        /// Fetches and loads the housing categories. 
        /// </summary>
        /// <returns>A <see cref="Task"/>.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        private Task LoadHousingCategories()
        {
            return Task.Run(
               async () =>
               {
                   HousingSearchFormInput.HousingCategories.Add(HousingCategoryViewModel.AllCategories);
                   HousingSearchFormInput.HousingCategories.AddRange(_autoMapper.Map<List<HousingCategoryViewModel>>(await _housingApi.GetHousingCategories()));
                   HousingSearchFormInput.SelectedCategoryId = HousingSearchFormInput.HousingCategories.First().HousingCategoryId;
               });
        }

        /// <summary>
		/// Fetches and loads the municipalties.
		/// </summary>
		/// <returns>A <see cref="Task"/>.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
		private Task LoadMunicipalities()
        {
            return Task.Run(
               async () =>
               {
                   HousingSearchFormInput.Municipalities.Add(MunicipalityViewModel.AllMunicipalities);
                   HousingSearchFormInput.Municipalities.AddRange(_autoMapper.Map<List<MunicipalityViewModel>>(await _housingApi.GetMunicipalities()));
                   HousingSearchFormInput.SelectedMunicipalityId = HousingSearchFormInput.Municipalities.First().MunicipalityId;
               });
        }

        


        /// <summary>
        /// Performs a search for housing objects and populates the collection.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        public async Task SearchHousings(int? pageNumber = null)
        {
            await InitalizeTask;
            int? municipalityFilter = HousingSearchFormInput.SelectedMunicipalityId != MunicipalityViewModel.AllMunicipalities.MunicipalityId ? HousingSearchFormInput.SelectedMunicipalityId : null;
            int? categoryFilter = HousingSearchFormInput.SelectedCategoryId != HousingCategoryViewModel.AllCategories.HousingCategoryId ? HousingSearchFormInput.SelectedCategoryId : null;
            int? offsetRows = pageNumber != null ? (pageNumber - 1) * _lastHousingSearchInput.NumberOfResultsPerPage : null;

            var result = await _housingApi.SearchHousings(maxNumberOfResultsPerPage: HousingSearchFormInput.NumberOfResultsPerPage, limitImageCountPerHousing: 3,
                municipalityId: municipalityFilter, housingCategoryId: categoryFilter,
                minPrice: HousingSearchFormInput.MinPrice, maxPrice: HousingSearchFormInput.MaxPrice,
                minLivingArea: HousingSearchFormInput.MinLivingArea, maxLivingArea: HousingSearchFormInput.MaxLivingArea,
                offsetRows: offsetRows);

            _lastHousingSearchInput = _autoMapper.Map<HousingSearchInputViewModel>(HousingSearchFormInput);
            HousingSearchResult = _autoMapper.Map<HousingSearchResultViewModel>(result);
            // TODO - Find a better way to retrieve the URLS
            HousingSearchResult.Housings.ForEach(x => x.Url = $"Housing/{x.HousingId}");
            HaveSearchedHousings = true;
        }

        #endregion
    }
}

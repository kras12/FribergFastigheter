using AutoMapper;
using FribergFastigheter.Client.Models.Housing;
using FribergFastigheter.Client.Services.FribergFastigheterApi;
using Microsoft.AspNetCore.Components;

namespace FribergFastigheter.Client.Services
{
    /// <summary>
    /// Performs housing searches and helps maintains state (search input and search results).
    /// </summary>
    /// <!-- Author: Jimmie -->
    /// <!-- Co Authors: -->
    public class HousingSearchService
    {
        #region Fields

        /// <summary>
        /// The injected auto mapper.
        /// </summary>
        private IMapper _autoMapper { get; set; }

        /// <summary>
        /// The injected service for the housing API.
        /// </summary>
        private IHousingApiService _housingApi { get; set; }

        /// <summary>
        /// Contains the input used for the last housing search.
        /// </summary>
        private HousingSearchInputViewModel _lastHousingSearchInput { get; set; } = new();        

        #endregion

        #region OtherProperties        

        /// <summary>
        /// True if the user have performed a housing search.
        /// </summary>
        public bool HaveSearchedHousings { get; set; } = false;

        /// <summary>
        /// Binds the input data from the housing search form.
        /// </summary>
        [SupplyParameterFromForm]
        public HousingSearchInputViewModel HousingSearchFormInput { get; set; } = new();

        /// <summary>
        /// The result of the housing search.
        /// </summary>
        public HousingSearchResultViewModel? HousingSearchResult { get; set; } = null;        

        /// <summary>
        /// The task for initializing service data. Wait for this task before starting to use the service. 
        /// </summary>
        public Task InitalizeTask { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="autoMapper">The injected auto mapper.</param>
        /// <param name="housingApi">The injected service for the housing API.</param>
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
                   var response = await _housingApi.GetHousingCategories();

                   if (response.Success)
                   {
                       HousingSearchFormInput.HousingCategories.Add(HousingCategoryViewModel.AllCategories);
                       HousingSearchFormInput.HousingCategories.AddRange(_autoMapper.Map<List<HousingCategoryViewModel>>(response.Value!));
                       HousingSearchFormInput.SelectedCategoryId = HousingSearchFormInput.HousingCategories.First().HousingCategoryId;
                   }
                   else
                   {
                       // TODO - Handle
                   }                   
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
                   var response = await _housingApi.GetMunicipalities();

                   if (response.Success)
                   {
                       HousingSearchFormInput.Municipalities.Add(MunicipalityViewModel.AllMunicipalities);
                       HousingSearchFormInput.Municipalities.AddRange(_autoMapper.Map<List<MunicipalityViewModel>>(response.Value!));
                       HousingSearchFormInput.SelectedMunicipalityId = HousingSearchFormInput.Municipalities.First().MunicipalityId;
                   }
                   else
                   {
                       // TODO - Handle
                   }
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

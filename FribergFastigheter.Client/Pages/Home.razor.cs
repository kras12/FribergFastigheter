using Microsoft.AspNetCore.Components;
using System.Diagnostics;
using System.Net.Http.Json;
using FribergFastigheter.Shared.Dto;
using FribergFastigheter.Client.Models;
using AutoMapper;
using FribergFastigheter.Client.Services.FribergFastigheterApi;

namespace FribergFastigheter.Client.Pages
{
    /// <summary>
    /// The website home page.
    /// </summary>
    /// <!-- Author: Jimmie -->
    /// <!-- Co Authors: -->
    public partial class Home : ComponentBase
    {
        #region Fields

        /// <summary>
        /// True if the user have performed a housing search.
        /// </summary>
        private bool _haveSearchedHousings = false;

        /// <summary>
        /// Contains the input used for the last housing search.
        /// </summary>
        private HousingSearchInputViewModel _lastHousingSearchInput { get; set; } = new();

        #endregion

        #region InjectedServiceProperties
#pragma warning disable CS8618

        /// <summary>
        /// The injected auto mapper.
        /// </summary>
        [Inject]
        private IMapper AutoMapper { get; set; }

        /// <summary>
        /// The injected service for the housing API.
        /// </summary>
        [Inject]
        private IHousingApiService HousingApi { get; set; }

#pragma warning restore CS8618
        #endregion

        #region OtherProperties

        /// <summary>
        /// The result of the housing search.
        /// </summary>
        private HousingSearchResultViewModel? HousingSearchResult { get; set; } = null;

        /// <summary>
        /// Binds the input data from the housing search form.
        /// </summary>
        [SupplyParameterFromForm]
        private HousingSearchInputViewModel HousingSearchFormInput { get; set; } = new();

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
                    HousingSearchFormInput.HousingCategories.AddRange(AutoMapper.Map<List<HousingCategoryViewModel>>(await HousingApi.GetHousingCategories()));
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
                   HousingSearchFormInput.Municipalities.AddRange(AutoMapper.Map<List<MunicipalityViewModel>>(await HousingApi.GetMunicipalities()));
                   HousingSearchFormInput.SelectedMunicipalityId = HousingSearchFormInput.Municipalities.First().MunicipalityId;
               });
        }

        /// <summary>
        /// Method invoked when the component is ready to start, having received its
        /// initial parameters from its parent in the render tree.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing any asynchronous operation.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        protected async override Task OnInitializedAsync()
		{
			await base.OnInitializedAsync();
            await Task.WhenAll(LoadHousingCategories(), LoadMunicipalities());
        }

        /// <summary>
        /// Performs a search for housing objects and populates the collection.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        private async Task SearchHousings(int? pageNumber = null)
        {
            int? municipalityFilter = HousingSearchFormInput.SelectedMunicipalityId != MunicipalityViewModel.AllMunicipalities.MunicipalityId ? HousingSearchFormInput.SelectedMunicipalityId : null;
            int? categoryFilter = HousingSearchFormInput.SelectedCategoryId != HousingCategoryViewModel.AllCategories.HousingCategoryId ? HousingSearchFormInput.SelectedCategoryId : null;
            int? offsetRows = pageNumber != null ? (pageNumber - 1) * _lastHousingSearchInput.NumberOfResultsPerPage : null;

			var result = await HousingApi.SearchHousings(maxNumberOfResultsPerPage: HousingSearchFormInput.NumberOfResultsPerPage, limitImageCountPerHousing: 3,
                municipalityId: municipalityFilter, housingCategoryId: categoryFilter, 
                minPrice: HousingSearchFormInput.MinPrice, maxPrice: HousingSearchFormInput.MaxPrice,
                minLivingArea: HousingSearchFormInput.MinLivingArea, maxLivingArea: HousingSearchFormInput.MaxLivingArea, 
                offsetRows: offsetRows);

            _lastHousingSearchInput = AutoMapper.Map<HousingSearchInputViewModel>(HousingSearchFormInput);
            HousingSearchResult = AutoMapper.Map<HousingSearchResultViewModel>(result);
            // TODO - Find a better way to retrieve the URLS
            HousingSearchResult.Housings.ForEach(x => x.Url = $"Housing/{x.HousingId}");
            _haveSearchedHousings = true;            
        }

        #endregion
    }	
}

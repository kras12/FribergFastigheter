using Microsoft.AspNetCore.Components;
using System.Diagnostics;
using System.Net.Http.Json;
using FribergFastigheter.Shared.Dto;
using FribergFastigheter.Client.Services.HousingApi;
using FribergFastigheter.Client.Models;
using AutoMapper;

namespace FribergFastigheter.Client.Pages
{
    /// <summary>
    /// The website home page component.
    /// </summary>
    /// <!-- Author: Jimmie -->
    /// <!-- Co Authors: -->
    public partial class Home : ComponentBase
    {
        #region Fields

        private bool _haveSearchedHousings = false;

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

        /// <summary>
        /// The injected service for the housing category API.
        /// </summary>
        [Inject]
        private IHousingCategoryApiService HousingCategoryApi { get; set; }

        /// <summary>
        /// The injected service for the municipality API.
        /// </summary>
        [Inject]
        private IMunicipalityApiService MunicipalityCategoryApi { get; set; }

#pragma warning restore CS8618
        #endregion

        #region OtherProperties

        /// <summary>
        /// The housing objects that were found in the search.
        /// </summary>
        public List<HousingViewModel> HousingResultList { get; set; } = new();

        /// <summary>
        /// The view model for the housing search form.
        /// </summary>
        [SupplyParameterFromForm]
        public HousingSearchInputViewModel HousingSearchInputViewModel { get; set; } = new();

        #endregion

        #region Methods

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

            var categories = await HousingCategoryApi.GetCategories();

            if (categories != null)
            {
                HousingSearchInputViewModel.HousingCategories.Add(HousingCategoryViewModel.AllCategories);
                HousingSearchInputViewModel.HousingCategories.AddRange(AutoMapper.Map<List<HousingCategoryViewModel>>(categories));
                HousingSearchInputViewModel.SelectedCategoryId = HousingSearchInputViewModel.HousingCategories.First().HousingCategoryId;
            }

            var municipalities = await MunicipalityCategoryApi.GetMunicipalities();

            if (municipalities != null)
            {
                HousingSearchInputViewModel.Municipalities.Add(MunicipalityViewModel.AllMunicipalities);
                HousingSearchInputViewModel.Municipalities.AddRange(AutoMapper.Map<List<MunicipalityViewModel>>(municipalities));
                HousingSearchInputViewModel.SelectedMunicipalityId = HousingSearchInputViewModel.Municipalities.First().MunicipalityId;
            }
        }

        /// <summary>
        /// Performs a search for housing objects and populates the collection.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        private async Task SearchHousings()
        {
            int? municipalityFilter = HousingSearchInputViewModel.SelectedMunicipalityId != MunicipalityViewModel.AllMunicipalities.MunicipalityId ? HousingSearchInputViewModel.SelectedMunicipalityId : null;
            int? categoryFilter = HousingSearchInputViewModel.SelectedCategoryId != HousingCategoryViewModel.AllCategories.HousingCategoryId ? HousingSearchInputViewModel.SelectedCategoryId : null;

            var housings = await HousingApi.SearchHousings(limitHousings: 10, limitImageCountPerHousing: 3,
                municipalityId: municipalityFilter, housingCategoryId: categoryFilter, 
                minPrice: HousingSearchInputViewModel.MinPrice, maxPrice: HousingSearchInputViewModel.MaxPrice,
                minLivingArea: HousingSearchInputViewModel.MinLivingArea, maxLivingArea: HousingSearchInputViewModel.MaxLivingArea);

            HousingResultList.Clear();

            if (housings != null)
            {
                HousingResultList = housings.Select(x => AutoMapper.Map<HousingViewModel>(x)).ToList();
            }

            _haveSearchedHousings = true;
        }

        #endregion
    }	
}

using AutoMapper;
using FribergFastigheter.Client.Models;
using FribergFastigheter.Client.Services.FribergFastigheterApi;
using FribergFastigheter.Shared.Dto;
using Microsoft.AspNetCore.Components;

namespace FribergFastigheter.Client.Components
{
    /// <summary>
    /// A component that creates a new housing object by displaying a form to the user.
    /// </summary>
    /// <!-- Author: Jimmie -->
    /// <!-- Co Authors: -->
    public partial class CreateHousing : ComponentBase
    {
        #region InjectedServiceProperties
#pragma warning disable CS8618

        /// <summary>
        /// The injected Auto Mapper service. 
        /// </summary>
        [Inject]
        private IMapper AutoMapper { get; set; }

        /// <summary>
        /// The injected housing API service.
        /// </summary>

        [Inject]
        private IBrokerFirmApiService BrokerFirmApiService { get; set; }

#pragma warning restore CS8618 
        #endregion

        #region Properties

        /// <summary>
        /// The ID of the broker firm the new housing belongs to.
        /// </summary>
        [Parameter]
        public int BrokerFirmId { get; set; }

        /// <summary>
        /// The ID of the broker the new housing belongs to.
        /// </summary>
        [Parameter]
        public int BrokerId { get; set; }

        /// <summary>
        /// The model bound to the form.
        /// </summary>
        [SupplyParameterFromForm]
        private CreateHousingViewModel CreateHousingInput { get; set; } = new();

        /// <summary>
        /// Event that is triggered when a new housing object has been created.
        /// </summary>
        [Parameter]
        public EventCallback<HousingViewModel> OnHousingCreated { get; set; }

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
                   var categories = await BrokerFirmApiService.GetHousingCategories();

                   if (categories != null)
                   {
                       CreateHousingInput.HousingCategories.AddRange(AutoMapper.Map<List<HousingCategoryViewModel>>(categories));
                       CreateHousingInput.SelectedCategoryId = CreateHousingInput.HousingCategories.First().HousingCategoryId;
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
                   var municipalities = await BrokerFirmApiService.GetMunicipalities();

                   if (municipalities != null)
                   {
                       CreateHousingInput.Municipalities.AddRange(AutoMapper.Map<List<MunicipalityViewModel>>(municipalities));
                       CreateHousingInput.SelectedMunicipalityId = CreateHousingInput.Municipalities.First().MunicipalityId;
                   }
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
        /// Event handler for the on valid form subsmission event. Creates a new housing object based on the form data.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
		private async Task OnValidSubmit()
        {
            CreateHousingInput.BrokerId = BrokerId;
            CreateHousingInput.BrokerFirmId = BrokerFirmId;
            var newHousing = await BrokerFirmApiService.CreateHousing(BrokerFirmId, AutoMapper.Map<CreateHousingDto>(CreateHousingInput));
            await OnHousingCreated.InvokeAsync(AutoMapper.Map<HousingViewModel>(newHousing));
        }        

        #endregion
    }
}

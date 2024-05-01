using AutoMapper;
using FribergFastigheter.Client.Models;
using FribergFastigheter.Client.Services.FribergFastigheterApi;
using FribergFastigheter.Shared.Dto;
using Microsoft.AspNetCore.Components;

namespace FribergFastigheter.Client.Components
{
    /// <summary>
    /// A component that provides functionality to edit a housing object by displaying a form to the user.
    /// </summary>
    /// <!-- Author: Jimmie -->
    /// <!-- Co Authors: -->
    public partial class EditHousing : ComponentBase
    {
        #region Fields

        /// <summary>
        /// A collection of housing categories to use in the form bound edit housing view model.
        /// </summary>
        private List<HousingCategoryViewModel> _housingCategories = new();

		/// <summary>
		/// A collection of municipalities to use in the form bound edit housing view model.
		/// </summary>
		private List<MunicipalityViewModel> _municipalities = new();

        #endregion

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

        #region OtherProperties        

        /// <summary>
        /// The model bound to the form.
        /// </summary>
        [SupplyParameterFromForm]
        private EditHousingViewModel? EditHousingInput { get; set; } = null;

#pragma warning disable CS8618
        /// <summary>
        /// The housing object to edit.
        /// </summary>
        [Parameter]
        public HousingViewModel Housing { get; set; }
#pragma warning restore CS8618 

        /// <summary>
        /// Event that is triggered when a housing object has been edited. 
        /// </summary>
        [Parameter]
        public EventCallback<HousingViewModel> OnHousingEditCancelled { get; set; }

        /// <summary>
        /// Event that is triggered when a housing object has been edited. 
        /// </summary>
        [Parameter]
        public EventCallback<HousingViewModel> OnHousingEdited { get; set; }

		#endregion

		#region Methods

		/// <summary>
		/// Creates a new edit housing model to be found to the edit form.
		/// </summary>
		/// <exception cref="ArgumentNullException"></exception>
		private void CreateEditHousingModel()
		{
			if (Housing == null)
			{
				throw new ArgumentNullException(nameof(Housing), "The housing object can't be null.");
			}

			EditHousingInput = AutoMapper.Map<EditHousingViewModel>(Housing);
            EditHousingInput.Municipalities = _municipalities;
            EditHousingInput.HousingCategories = _housingCategories;
		}

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
                       _housingCategories = AutoMapper.Map<List<HousingCategoryViewModel>>(categories);
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
                       _municipalities = AutoMapper.Map<List<MunicipalityViewModel>>(municipalities);
                   }
               });
        }

        /// <summary>
        /// Event handler for the cancel housing edit button. 
        /// </summary>
        /// <returns></returns>
        private Task OnCancelHousingEditButtonClicked()
        {
            return OnHousingEditCancelled.InvokeAsync(Housing);
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
        /// Method invoked when the component has received parameters from its parent in 
        /// the render tree, and the incoming values have been assigned to properties.
        /// </summary>
        /// <returns>A System.Threading.Tasks.Task representing any asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
			CreateEditHousingModel();
        }

        /// <summary>
        /// Event handler for the on valid form subsmission event. Creates a new housing object based on the form data.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
		private async Task OnValidSubmit()
        {
            await BrokerFirmApiService.UpdateHousing(AutoMapper.Map<EditHousingDto>(EditHousingInput));
            AutoMapper.Map(EditHousingInput!, Housing);
			Housing.Municipality = _municipalities.First(x => x.MunicipalityId == EditHousingInput!.SelectedMunicipalityId);
			Housing.Category = _housingCategories.First(x => x.HousingCategoryId == EditHousingInput!.SelectedCategoryId);
            await OnHousingEdited.InvokeAsync(Housing);
        }        

        #endregion
    }
}

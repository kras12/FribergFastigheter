using AutoMapper;
using FribergFastigheter.Client.Models.Housing;
using FribergFastigheter.Client.Models.Image;
using FribergFastigheter.Client.Services.FribergFastigheterApi;
using FribergFastigheter.Shared.Dto.Housing;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace FribergFastigheter.Client.Components.Housing
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
        /// A collection of images the user have chosen to delete.
        /// </summary>
        private List<ImageViewModel> _imagesToDelete = new();

		/// <summary>
		/// A collection of municipalities to use in the form bound edit housing view model.
		/// </summary>
		private List<MunicipalityViewModel> _municipalities = new();

        /// <summary>
        ///  A collection of images to upload. 
        /// </summary>
        private List<IBrowserFile> _uploadedFiles = new();

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
        /// Fetches and loads all images for the housing object.
        /// </summary>
        /// <returns></returns>
        private Task LoadAllImages()
        {
            return Task.Run(
               async () =>
               {
                   Housing.Images = AutoMapper.Map<List<ImageViewModel>>(await BrokerFirmApiService.GetHousingImages(Housing.HousingId));                  
               });
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
                   _housingCategories = AutoMapper.Map<List<HousingCategoryViewModel>>(await BrokerFirmApiService.GetHousingCategories());                 
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
                    _municipalities = AutoMapper.Map<List<MunicipalityViewModel>>(await BrokerFirmApiService.GetMunicipalities());
               });
        }

        /// <summary>
        /// Event handler for the cancel housing edit button. 
        /// </summary>
        /// <returns></returns>
        private Task OnCancelHousingEditButtonClicked()
        {
            Housing.Images.AddRange(_imagesToDelete);
            _imagesToDelete.Clear();
            return OnHousingEditCancelled.InvokeAsync(Housing);
        }

        /// <summary>
        /// Event handler for when the delete image button was clicked. 
        /// </summary>
        /// <param name="image">The image to delete.</param>
        /// <returns>A <see cref="Task"/>.</returns>
        private void OnDeleteImageButtonClickedEventHandler(ImageViewModel image)
        {
            _imagesToDelete.Add(image);
            Housing.Images.Remove(image);
        }

        /// <summary>
        /// Event handler to handle changes for the input file element in the form. 
        /// </summary>
        /// <param name="e">The event arguments.</param>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        private void OnFileUploadChanged(InputFileChangeEventArgs e)
        {
            // TODO - Move image types to another class and perhaps in the share project.
            List<string> allowedImageTypes = new()
            {
                "image/jpeg",
                "image/png"
            };

            _uploadedFiles = e.GetMultipleFiles(maximumFileCount: 100)
                .Where(x => allowedImageTypes.Contains(x.ContentType))
                .ToList();
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
            await Task.WhenAll(LoadHousingCategories(), LoadMunicipalities(), LoadAllImages());
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
            await BrokerFirmApiService.DeleteImages(Housing.HousingId, _imagesToDelete.Select(x => x.ImageId).ToList());
            Housing.Images.AddRange(await UploadImages());
            await OnHousingEdited.InvokeAsync(Housing);
        }

        /// <summary>
        /// Uploads images for a housing object if the user have selected any images. 
        /// </summary>
        /// <returns>A collection of <see cref="ImageViewModel"/> objects for the uploaded images.</returns>
        private async Task<List<ImageViewModel>> UploadImages()
        {
            if (_uploadedFiles.Count > 0)
            {
                var result = await BrokerFirmApiService.UploadHousingImages(Housing.HousingId, _uploadedFiles);
                return result != null ? AutoMapper.Map<List<ImageViewModel>>(result) : new();
            }

            return new();
        }

        #endregion
    }
}

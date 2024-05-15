using AutoMapper;
using FribergFastigheter.Client.Models.Housing;
using FribergFastigheter.Client.Models.Image;
using FribergFastigheter.Client.Services.FribergFastigheterApi;
using FribergFastigheter.Shared.Constants;
using FribergFastigheter.Shared.Dto.Housing;
using FribergFastigheter.Shared.Services.AuthorizationHandlers.Housing.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
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
        /// The injected authorization service. 
        /// </summary>
        [Inject]
        private IAuthorizationService AuthorizationService { get; set; }

        /// <summary>
        /// The injected housing API service.
        /// </summary>

        [Inject]
        private IBrokerFirmApiService BrokerFirmApiService { get; set; }

#pragma warning restore CS8618 
        #endregion

        #region ParameterProperties

        /// <summary>
        /// The cascaded authentication state task.
        /// </summary>
        [CascadingParameter]
#pragma warning disable CS8618 
        private Task<AuthenticationState> AuthenticationStateTask { get; set; }
#pragma warning restore CS8618 

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
        /// Sends an request to the API to delete chosen images (if any) from the housing object. 
        /// </summary>
        /// <returns>A <see cref="Task"/> representing an async operation.</returns>
        /// <exception cref="UnauthorizedAccessException"></exception>
        private async Task DeleteImages()
        {
            var user = (await AuthenticationStateTask).User;

            if (_imagesToDelete.Count > 0)
            {
                var deleteImageAuthorizationData = new DeleteHousingImageAuthorizationData(Housing.Broker.BrokerFirm.BrokerFirmId, Housing.Broker.BrokerId);
                var deleteImageAuthorizationResult = await AuthorizationService.AuthorizeAsync(user, deleteImageAuthorizationData, ApplicationPolicies.CanDeleteHousingImageResource);

                if (deleteImageAuthorizationResult.Succeeded)
                {
                    var response = await BrokerFirmApiService.DeleteHousingImages(Housing.HousingId, _imagesToDelete.Select(x => x.ImageId).ToList());

                    if (!response.Success)
                    {
                        // TODO - handle
                    }
                }
                else
                {
                    throw new UnauthorizedAccessException();
                }
            }
        }

        /// <summary>
        /// Sends an request to the API to update the housing object. 
        /// </summary>
        /// <returns>A <see cref="Task"/> representing an async operation.</returns>
        /// <exception cref="UnauthorizedAccessException"></exception>
        private async Task EditHousingData()
        {
            var user = (await AuthenticationStateTask).User;
            var housingAuthorizationData = new EditHousingAuthorizationData(existingHousingBrokerFirmId: Housing.Broker.BrokerFirm.BrokerFirmId,
                existingHousingBrokerId: Housing.Broker.BrokerId, newHousingBrokerId: EditHousingInput!.BrokerId);
            var housingAuthorizationResult = await AuthorizationService.AuthorizeAsync(user, housingAuthorizationData, ApplicationPolicies.CanEditHousingResource);

            if (housingAuthorizationResult.Succeeded)
            {
                var response = await BrokerFirmApiService.UpdateHousing(AutoMapper.Map<EditHousingDto>(EditHousingInput));

                if (response.Success)
                {
                    AutoMapper.Map(EditHousingInput!, Housing);
                    Housing.Municipality = _municipalities.First(x => x.MunicipalityId == EditHousingInput!.SelectedMunicipalityId);
                    Housing.Category = _housingCategories.First(x => x.HousingCategoryId == EditHousingInput!.SelectedCategoryId);
                }
                else
                {
                    // TODO - Handle
                }
            }
            else
            {
                throw new UnauthorizedAccessException();
            }
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
                   var response = await BrokerFirmApiService.GetHousingImages(Housing.HousingId);

                   if (response.Success)
                   {
                       Housing.Images = AutoMapper.Map<List<ImageViewModel>>(response.Value!);
                   }
                   else
                   {
                       // TODO - handle
                   }                   
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
                   var response = await BrokerFirmApiService.GetHousingCategories();

                   if (response.Success)
                   {
                       _housingCategories = AutoMapper.Map<List<HousingCategoryViewModel>>(response.Value!);
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
                   var response = await BrokerFirmApiService.GetMunicipalities();

                   if (response.Success)
                   {
                       _municipalities = AutoMapper.Map<List<MunicipalityViewModel>>(response.Value!);
                   }
                   else
                   {
                       // Todo - Handle
                   }
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

            if (Housing == null)
            {
                throw new ArgumentNullException($"The paramater '{Housing}' can't be null.", nameof(Housing));
            }

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
                try
                {
                    await EditHousingData();
                }
                catch (UnauthorizedAccessException)
                {
                    // TODO - Show message
                }

                try
                {
                    await DeleteImages();
                }
                catch (UnauthorizedAccessException)
                {
                    // TODO - Show message
                }

                try
                {
                    await UploadImages();
                }
                catch (UnauthorizedAccessException)
                {
                    // TODO - Show message
                }

                await OnHousingEdited.InvokeAsync(Housing);
        }             

        /// <summary>
        /// Sends an request to the API to upload chosen images (if any) to the housing object. 
        /// </summary>
        /// <returns>A <see cref="Task"/> representing an async operation.</returns>
        /// <exception cref="UnauthorizedAccessException"></exception>
        private async Task UploadImages()
        {
            if (_uploadedFiles.Count > 0)
            {
                var user = (await AuthenticationStateTask).User;
                var imageAuthorizationData = new CreateHousingImageAuthorizationData(Housing.Broker.BrokerFirm.BrokerFirmId, Housing.Broker.BrokerId);
                var imageAuthorizationResult = await AuthorizationService.AuthorizeAsync(user, imageAuthorizationData, ApplicationPolicies.CanCreateHousingImageResource);

                if (imageAuthorizationResult.Succeeded)
                {
                    var response = await BrokerFirmApiService.UploadHousingImages(Housing.HousingId, _uploadedFiles);

                    if (response.Success)
                    {
                        var innerResponse = await BrokerFirmApiService.UploadHousingImages(Housing.HousingId, _uploadedFiles);

                        if (innerResponse.Success)
                        {
                            Housing.Images.AddRange(AutoMapper.Map<List<ImageViewModel>>(innerResponse.Value!));
                        }
                        else
                        {
                            // Todo - Handle
                        }

                    }
                    else
                    {
                        // Todo - Handle
                    }
                }
                else
                {
                    throw new UnauthorizedAccessException();
                }
            }
        }

        #endregion
    }
}

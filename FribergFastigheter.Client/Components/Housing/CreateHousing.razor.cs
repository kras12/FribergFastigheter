using AutoMapper;
using FribergFastigheter.Client.Models.Housing;
using FribergFastigheter.Client.Services.FribergFastigheterApi;
using FribergFastigheter.Shared.Constants;
using FribergFastigheter.Shared.Dto.Housing;
using FribergFastigheter.Shared.Dto.Image;
using FribergFastigheter.Shared.Services.AuthorizationHandlers.Housing.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;

namespace FribergFastigheter.Client.Components.Housing
{
    /// <summary>
    /// A component that creates a new housing object by displaying a form to the user.
    /// </summary>
    /// <!-- Author: Jimmie -->
    /// <!-- Co Authors: -->
    public partial class CreateHousing : ComponentBase
    {
        #region Fields

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

        #region Properties

        /// <summary>
        /// The authentication state task. 
        /// </summary>
        [CascadingParameter]
#pragma warning disable CS8618 
		private Task<AuthenticationState> AuthenticationStateTask { get; set; }
#pragma warning restore CS8618 

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

        /// <summary>
        /// Event that is triggered when a the housing creation was cancelled. 
        /// </summary>
        [Parameter]
        public EventCallback OnHousingCreationCancelled { get; set; }

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
                   CreateHousingInput.HousingCategories.AddRange(AutoMapper.Map<List<HousingCategoryViewModel>>(await BrokerFirmApiService.GetHousingCategories()));
                   CreateHousingInput.SelectedCategoryId = CreateHousingInput.HousingCategories.First().HousingCategoryId;
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
                    CreateHousingInput.Municipalities.AddRange(AutoMapper.Map<List<MunicipalityViewModel>>(await BrokerFirmApiService.GetMunicipalities()));
                    CreateHousingInput.SelectedMunicipalityId = CreateHousingInput.Municipalities.First().MunicipalityId;
               });
        }

        /// <summary>
        /// Event handler for when the cancel housing creation button was clicked. 
        /// </summary>
        /// <returns></returns>
        private Task OnCancelCreateHousingButtonClicked()
        {
            return OnHousingCreationCancelled.InvokeAsync(null);
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
            await Task.WhenAll(LoadHousingCategories(), LoadMunicipalities());
        }

        /// <summary>
        /// Event handler for the on valid form subsmission event. Creates a new housing object based on the form data.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
		private async Task OnValidSubmit()
        {
            var user = (await AuthenticationStateTask).User;
            var brokerId = int.Parse(user.FindFirst(ApplicationUserClaims.BrokerId)!.Value);
            var authorizationData = new CreateHousingAuthorizationData(newHousingBrokerId: brokerId);
            var result = await AuthorizationService.AuthorizeAsync(user, authorizationData, ApplicationPolicies.CanCreateHousingResource);

            if (result.Succeeded)
            {
                CreateHousingInput.BrokerId = int.Parse((await AuthenticationStateTask).User.FindFirst(x => x.Type == ApplicationUserClaims.BrokerId)!.Value);
                var newHousing = await BrokerFirmApiService.CreateHousing(AutoMapper.Map<CreateHousingDto>(CreateHousingInput));
                newHousing.Images = await UploadImages(newHousing.HousingId);
                await OnHousingCreated.InvokeAsync(AutoMapper.Map<HousingViewModel>(newHousing));
            }
            else
            {
                // TODO - show message.
            }
        }        

        /// <summary>
        /// Uploads images for a housing object if the user have selected any images. 
        /// </summary>
        /// <param name="housingId">The ID of the housing object to upload the images for.</param>
        /// <returns>A collection of <see cref="ImageDto"/> objects for the uploaded images.</returns>
        private async Task<List<ImageDto>> UploadImages(int housingId)
        {
            if (_uploadedFiles.Count > 0)
            {
                return await BrokerFirmApiService.UploadHousingImages(housingId, _uploadedFiles);
            }

            return new List<ImageDto>();
        }

        #endregion
    }
}

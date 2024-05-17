using AutoMapper;
using FribergFastigheter.Client.Models.Broker;
using FribergFastigheter.Client.Models.Image;
using FribergFastigheter.Client.Services.FribergFastigheterApi;
using FribergFastigheter.Shared.Constants;
using FribergFastigheter.Shared.Dto.Broker;
using FribergFastigheter.Shared.Dto.Image;
using FribergFastigheter.Shared.Services.AuthorizationHandlers.Broker.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;

namespace FribergFastigheter.Client.Components.Broker
{
    /// <summary>
    /// A component that creates a new broker object by displaying a form to the user.
    /// </summary>
    /// <!-- Author: Marcus -->
    /// <!-- Co Authors: -->
    /// 
    public partial class CreateBroker : ComponentBase
    {
        #region Fields

        /// <summary>
        /// A collection of validation errors returned from the API.
        /// </summary>
        private List<string> _apiValidationErrors = new List<string>();

        private IBrowserFile? UploadedProfileImage = null;

        #endregion

        #region InjectedServiceProperties

        /// <summary>
        /// The injected housing API service.
        /// </summary>
        [Inject]
        private IBrokerFirmApiService BrokerFirmApiService { get; set; }
        /// <summary>
        /// The injected Auto Mapper service. 
        /// </summary>
        [Inject]
        private IMapper Mapper { get; set; }
        /// <summary>
        /// The injected authorization service. 
        /// </summary>
        [Inject]
        private IAuthorizationService AuthorizationService { get; set; }

        #endregion

        #region Properties

        /// <summary>
        /// The model bound to the form.
        /// </summary>
        [SupplyParameterFromForm]
        private CreateBrokerViewModel CreateBrokerInput { get; set; } = new();

        /// <summary>
        /// Event that is triggered when a new broker object has been created.
        /// </summary>
        [Parameter]
        public EventCallback<BrokerViewModel> OnBrokerCreated { get; set; }
        [Parameter]
        public EventCallback<bool> CloseCreateNewBroker { get; set; }

        /// <summary>
        /// The authentication state task. 
        /// </summary>
        [CascadingParameter]
#pragma warning disable CS8618
        private Task<AuthenticationState> AuthenticationStateTask { get; set; }
#pragma warning restore CS8618 

        #endregion

        #region Methods

        protected async override Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
        }

        /// <summary>
        /// Event handler for the on valid form subsmission event. Creates a new broker object based on the form data.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
		private async Task OnValidSubmit()
        {
            var user = (await AuthenticationStateTask).User;
            var brokerId = int.Parse(user.FindFirst(ApplicationUserClaims.BrokerId)!.Value);
            var authorizationData = new CreateBrokerAuthorizationData(brokerId);
            var result = await AuthorizationService.AuthorizeAsync(user, authorizationData, ApplicationPolicies.CanCreateBrokerResource);

            if (result.Succeeded)
            {
                var response = await BrokerFirmApiService.CreateBroker(Mapper.Map<CreateBrokerDto>(CreateBrokerInput));

                if (response.Success)
                {
                    var newBroker = response.Value;
                    var newBrokerViewModel = Mapper.Map<BrokerViewModel>(newBroker);
                    newBrokerViewModel.ProfileImage = Mapper.Map<ImageViewModel>(await UploadProfileImage(newBroker!.BrokerId));
                    await OnBrokerCreated.InvokeAsync(newBrokerViewModel);
                }
                else
                {
                    _apiValidationErrors = response.GetErrorDescriptionsAsList();
                }
            }
            else
            {
                // TODO - show message.
            }

        }

        private async Task CloseCreateForm()
        {
            await CloseCreateNewBroker.InvokeAsync(true);
        }

        /// <summary>
        /// Uploads images for a housing object if the user have selected any images. 
        /// </summary>
        /// <param name="housingId">The ID of the housing object to upload the images for.</param>
        /// <returns>A collection of <see cref="ImageDto"/> objects for the uploaded images.</returns>
        private async Task<ImageDto?> UploadProfileImage(int brokerId)
        {
            if (UploadedProfileImage != null)
            {
                var response = await BrokerFirmApiService.UploadBrokerProfileImage(brokerId, UploadedProfileImage);

                if (response.Success)
                {
                    return response.Value!;
                }
                else
                {
                    // TODO - handle
                }
            }

            return null;
        }

        private void OnFileUploadChanged(InputFileChangeEventArgs e)
        {
            // TODO - Move image types to another class and perhaps in the share project.
            List<string> allowedImageTypes = new()
            {
                "image/jpeg",
                "image/png"
            };

            UploadedProfileImage = e.GetMultipleFiles(maximumFileCount: 1)
                .FirstOrDefault(x => allowedImageTypes.Contains(x.ContentType));
        }
        #endregion
    }
}

using AutoMapper;
using FribergFastigheter.Client.Models.Broker;
using FribergFastigheter.Client.Models.Image;
using FribergFastigheter.Client.Services.FribergFastigheterApi;
using FribergFastigheter.Shared.Constants;
using FribergFastigheter.Shared.Dto.Broker;
using FribergFastigheter.Shared.Services.AuthorizationHandlers.Broker.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;

namespace FribergFastigheter.Client.Components.Broker
{

    /// <summary>
    /// A component that provides functionality to edit a broker object by displaying a form to the user.
    /// </summary>
    /// <!-- Author: Marcus -->
    /// <!-- Co Authors: -->
    public partial class EditBroker : ComponentBase
    {
        #region Fields

        /// <summary>
        /// A collection of validation errors returned from the API.
        /// </summary>
        private List<string> _apiValidationErrors = new List<string>();

        /// <summary>
        /// True if the user have chosen to delete the profile image.
        /// </summary>
        private bool _deleteProfileImage = false;
        private IBrowserFile? _uploadedProfileImage = null;

        #endregion

        #region InjectedServiceProperties

        /// <summary>
        /// The injected authorization service. 
        /// </summary>
        [Inject]
        private IAuthorizationService AuthorizationService { get; set; }

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

        #endregion

        #region Properties

        [CascadingParameter]
        private Task<AuthenticationState> AuthenticationStateTask { get; set; }

        [Parameter]
        public BrokerViewModel Broker { get; set; }

        [SupplyParameterFromForm]
        private EditBrokerViewModel BrokerFormInput { get; set; }

        [Parameter]
        public EventCallback CloseEditBroker { get; set; }

        [Parameter]
        public EventCallback<BrokerViewModel> OnBrokerEdited { get; set; }

        #endregion

        #region Methods

        protected async override Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();   
        }

        private async Task CreateEditBrokerModel()
        {
            if (Broker == null)
            {
                throw new ArgumentNullException(nameof(Broker), "The broker object can't be null.");
            }

            BrokerFormInput = AutoMapper.Map<EditBrokerViewModel>(Broker);           
        }

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            await CreateEditBrokerModel();
        }

        private async Task OnValidSubmit()
        {
            var brokerFirmId = int.Parse((await AuthenticationStateTask).User.FindFirst(x => x.Type == ApplicationUserClaims.BrokerFirmId)!.Value);

            var authorizationData = new EditBrokerAuthorizationData(
                existingBroker: AutoMapper.Map<BrokerDto>(Broker),
                newBroker: AutoMapper.Map<EditBrokerDto>(BrokerFormInput));

            if (await CheckPolicy(ApplicationPolicies.CanEditBrokerResource, authorizationData))
            {
                var response = await BrokerFirmApiService.EditBroker(authorizationData.NewBroker);

                if (response.Success)
                {
                    AutoMapper.Map(response.Value, Broker);

                        if (_deleteProfileImage)
                        {
                            var innerResponse = await BrokerFirmApiService.DeleteBrokerProfileImage(Broker.BrokerId);

                        if (innerResponse.Success)
                        {
                            Broker.ProfileImage = null;
                        }
                        else
                        {
                            // TODO - handle
                        }
                    }
                    if (_uploadedProfileImage != null)
                    {
                        Broker.ProfileImage = await UploadImages(Broker.BrokerId);
                    }
                    await OnBrokerEdited.InvokeAsync(Broker);
                }
                else
                {
                    _apiValidationErrors = response.GetErrorDescriptionsAsList();
                }
            }
            else
            {
                // TODO - Show message
            }
        }
 
        private async Task CloseEditForm()
        {
            ///TODO Make an reset method instead
            await CloseEditBroker.InvokeAsync();
        }

        private void OnDeleteImageButtonClickedEventHandler()
        {
            BrokerFormInput.ProfileImage = null;
            _deleteProfileImage = true;
            StateHasChanged();
        }

        private void OnFileUploadChanged(InputFileChangeEventArgs e)
        {
            // TODO - Move image types to another class and perhaps in the share project.
            List<string> allowedImageTypes = new()
            {
                "image/jpeg",
                "image/png"
            };

            _uploadedProfileImage = e.GetMultipleFiles(maximumFileCount: 1)
                .FirstOrDefault(x => allowedImageTypes.Contains(x.ContentType));
        }

        private async Task<ImageViewModel> UploadImages(int brokerId)
        {
            if (_uploadedProfileImage == null)
            {
                throw new InvalidOperationException("No image to upload was found");
            }

            var response = await BrokerFirmApiService.UploadBrokerProfileImage(brokerId, _uploadedProfileImage);

            if (response.Success)
            {
                _uploadedProfileImage = null;
                return AutoMapper.Map<ImageViewModel>(response.Value!);
            }
            else
            {
                // TODO - handle
                throw new Exception($"Failed to upload profile image: {response.GetErrorsAsString()}");
            }            
        }

        private async Task<bool> CheckPolicy(string policy)
        {
            var user = (await AuthenticationStateTask).User;
            var result = await AuthorizationService.AuthorizeAsync(user, policy);
            return result.Succeeded;
        }

        private async Task<bool> CheckPolicy<T>(string policy, T resorce)
        {
            var user = (await AuthenticationStateTask).User;
            var result = await AuthorizationService.AuthorizeAsync(user, resorce, policy);
            return result.Succeeded;
        }

        #endregion
    }
}

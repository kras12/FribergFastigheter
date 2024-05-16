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
        private AdminEditBrokerViewModel AdminBrokerInput { get; set; }

        [SupplyParameterFromForm]
        private EditBrokerViewModel BrokerInput { get; set; }

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
            if(await CheckPolicy(ApplicationPolicies.BrokerAdmin))
            {
                AdminBrokerInput = AutoMapper.Map<AdminEditBrokerViewModel>(Broker);
            }
            else if (await CheckPolicy(ApplicationPolicies.Broker))
            {
                BrokerInput = AutoMapper.Map<EditBrokerViewModel>(Broker);
            }
           
        }

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            await CreateEditBrokerModel();
        }

        private async Task OnValidSubmit()
        {
            var authorizationData = new EditBrokerAuthorizationData(existingBrokerBrokerFirmId: Broker.BrokerFirm.BrokerFirmId,
                existingBrokerBrokerId: Broker.BrokerId);
  
                if (await CheckPolicy(ApplicationPolicies.BrokerAdmin) && await CheckPolicy(ApplicationPolicies.CanEditBrokerResource, authorizationData))
                {
                    var response = await BrokerFirmApiService.AdminEditBroker(Broker.BrokerId, AutoMapper.Map<AdminEditBrokerDto>(AdminBrokerInput));

                    if (response.Success)
                    {
                        AutoMapper.Map(AdminBrokerInput!, Broker);

                        if (_deleteProfileImage)
                        {
                            var innerResponse = await BrokerFirmApiService.DeleteBrokerProfileImage(Broker.BrokerId);

                            if (!innerResponse.Success)
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
                        // TODO - Show message
                    }
                }      
                
                if (await CheckPolicy(ApplicationPolicies.Broker) && await CheckPolicy(ApplicationPolicies.CanEditBrokerResource, authorizationData))
                {
                    var response = await BrokerFirmApiService.EditBroker(Broker.BrokerId, AutoMapper.Map<EditBrokerDto>(BrokerInput));

                    if (response.Success)
                    {
                        AutoMapper.Map(BrokerInput!, Broker);

                        if (_deleteProfileImage)
                        {
                            var innerResponse = await BrokerFirmApiService.DeleteBrokerProfileImage(Broker.BrokerId);

                            if (!innerResponse.Success)
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
                        // TODO - Show message
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

        private async Task OnDeleteImageButtonClickedEventHandler(ImageViewModel image)
        {
            if (await CheckPolicy(ApplicationPolicies.BrokerAdmin))
            {
                AdminBrokerInput.ProfileImage = null;
            }
            else if (await CheckPolicy(ApplicationPolicies.Broker))
            {
                BrokerInput.ProfileImage = null;
            }
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

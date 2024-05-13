using AutoMapper;
using FribergFastigheter.Client.Models.Broker;
using FribergFastigheter.Client.Models.Image;
using FribergFastigheter.Client.Services.FribergFastigheterApi;
using FribergFastigheter.Shared.Dto.Broker;
using Microsoft.AspNetCore.Components;
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

        [Parameter]
        public BrokerViewModel Broker { get; set; }

        [SupplyParameterFromForm]
        private AdminEditBrokerViewModel BrokerInput { get; set; }

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

        private void CreateEditBrokerModel()
        {
            if (Broker == null)
            {
                throw new ArgumentNullException(nameof(Broker), "The broker object can't be null.");
            }

            BrokerInput = AutoMapper.Map<AdminEditBrokerViewModel>(Broker);
        }

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            CreateEditBrokerModel();
        }

        private async Task OnValidSubmit()
        {
            await BrokerFirmApiService.AdminEditBroker(Broker.BrokerId, AutoMapper.Map<AdminEditBrokerDto>(BrokerInput));
            AutoMapper.Map(BrokerInput!, Broker);            

            if (_deleteProfileImage)
            {
                await BrokerFirmApiService.DeleteBrokerProfileImage(Broker.BrokerId);
            }            
            if(_uploadedProfileImage != null)
            {
                Broker.ProfileImage = await UploadImages(Broker.BrokerId);
            }
            await OnBrokerEdited.InvokeAsync(Broker);   
        }
 
        private async Task CloseEditForm()
        {
            ///TODO Make an reset method instead
            await CloseEditBroker.InvokeAsync();
        }

        private void OnDeleteImageButtonClickedEventHandler(ImageViewModel image)
        {
            BrokerInput.ProfileImage = null;
            _deleteProfileImage = true;
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
            var result = await BrokerFirmApiService.UploadBrokerProfileImage(brokerId, _uploadedProfileImage);
            _uploadedProfileImage = null;
            return result != null ? AutoMapper.Map<ImageViewModel>(result) : new ImageViewModel();
            
        }
        #endregion
    }
}

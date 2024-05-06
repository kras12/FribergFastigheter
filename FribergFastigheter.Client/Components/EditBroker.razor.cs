using AutoMapper;
using FribergFastigheter.Client.Models;
using FribergFastigheter.Client.Services.FribergFastigheterApi;
using FribergFastigheter.Shared.Dto;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace FribergFastigheter.Client.Components
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
        private bool _deleteProfileImage = new();
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
        public int BrokerFirmId { get; set; } = 1;
        [SupplyParameterFromForm]
        private EditBrokerViewModel BrokerInput { get; set; } = null;

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

            BrokerInput = AutoMapper.Map<EditBrokerViewModel>(Broker);
        }

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            CreateEditBrokerModel();
        }

        private async Task OnValidSubmit()
        {
            BrokerInput.BrokerFirm.BrokerFirmId = BrokerFirmId;
            await BrokerFirmApiService.UpdateBroker(Broker.BrokerId, AutoMapper.Map<EditBrokerDto>(BrokerInput));
            AutoMapper.Map(BrokerInput!, Broker);
            if (_deleteProfileImage)
            {
                await BrokerFirmApiService.DeleteBrokerProfileImage(Broker.BrokerFirm.BrokerFirmId, Broker.BrokerId);
            }            
            if(_uploadedProfileImage != null)
            {
                Broker.ProfileImage = await UploadImages(Broker.BrokerId);
            }
            await OnBrokerEdited.InvokeAsync(Broker);   
        }
 
        private async Task CloseEditForm()
        {
            await CloseEditBroker.InvokeAsync();
        }

        private void OnDeleteImageButtonClickedEventHandler(ImageViewModel image)
        {
            _deleteProfileImage = true;
            Broker.ProfileImage = null;
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
            var result = await BrokerFirmApiService.UploadBrokerProfileImage(BrokerFirmId, brokerId, _uploadedProfileImage);
            _uploadedProfileImage = null;
            return result != null ? AutoMapper.Map<ImageViewModel>(result) : new ImageViewModel();
            
        }
        #endregion
    }
}

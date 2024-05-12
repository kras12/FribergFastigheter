﻿using AutoMapper;
using FribergFastigheter.Client.Models.Broker;
using FribergFastigheter.Client.Models.Image;
using FribergFastigheter.Client.Services.FribergFastigheterApi;
using FribergFastigheter.Shared.Dto.Broker;
using FribergFastigheter.Shared.Dto.Image;
using Microsoft.AspNetCore.Components;
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
            var newBroker = await BrokerFirmApiService.AdminCreateBroker(Mapper.Map<RegisterBrokerDto>(CreateBrokerInput));
            var newBrokerViewModel = Mapper.Map<BrokerViewModel>(newBroker);
            newBrokerViewModel.ProfileImage = Mapper.Map<ImageViewModel>(await UploadProfileImage(newBroker!.BrokerId));
            await OnBrokerCreated.InvokeAsync(newBrokerViewModel);
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
                return await BrokerFirmApiService.UploadBrokerProfileImage(brokerId, UploadedProfileImage);
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
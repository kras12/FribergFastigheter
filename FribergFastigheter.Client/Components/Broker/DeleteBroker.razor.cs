﻿using AutoMapper;
using FribergFastigheter.Client.Models.Broker;
using FribergFastigheter.Client.Models.Image;
using FribergFastigheter.Client.Services.FribergFastigheterApi;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using static FribergFastigheter.Client.Components.ConfirmButton;

namespace FribergFastigheter.Client.Components.Broker
{
    /// <summary>
    /// A component that displays a delete button and handles deletion of a broker object. 
    /// </summary>
    /// <!-- Author: Marcus -->
    /// <!-- Co Authors: -->
    /// 
    public partial class DeleteBroker : ComponentBase
    {
        #region InjectedServiceProperties

        /// <summary>
        /// The injected auto mapper service. 
        /// </summary>
        [Inject]
        private IMapper AutoMapper { get; set; }
        
        /// <summary>
        /// The injected broker firm API service. 
        /// </summary>
        [Inject]
        private IBrokerFirmApiService BrokerFirmApiService { get; set; }

        /// <summary>
        /// Injected JavaScript runtime.
        /// </summary>
        [Inject]
        public IJSRuntime JsRuntime { get; set; }

        #endregion

        #region Properties

        /// <summary>
        /// The broker object that can be deleted. 
        /// </summary>
        [Parameter]
        public BrokerViewModel Broker { get; set; }

        /// <summary>
        /// Event triggers when the broker object have been deleted.
        /// </summary>
        [Parameter]
        public EventCallback<BrokerViewModel> OnBrokerDeleted { get; set; }


        #endregion

        #region Methods


        /// <summary>
        /// Event handler for when the user makes a choice in the confirm delete broker dialog box. 
        /// </summary>
        /// <param name="result">The result of the user action.</param>
        /// <returns>A <see cref="Task"/>.</returns>
        private async Task OnConfirmDeleteBrokerEventHandler(DialogResults result)
        {

            if (result == DialogResults.UserConfirmed)
            {
                ///TODO 
                if (Broker.ProfileImage == null)
                {
                    Broker.ProfileImage = new ImageViewModel();
                }
                await BrokerFirmApiService.DeleteBroker(Broker.BrokerId);
                await OnBrokerDeleted.InvokeAsync(Broker);
            } 
        }

        protected override async Task OnInitializedAsync()
        {
            Broker.HousingsCount = await BrokerFirmApiService.GetHousingCountByBrokerId(Broker.BrokerId);
        }

        #endregion
    }
}
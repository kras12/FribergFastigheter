using AutoMapper;
using FribergFastigheter.Client.Models;
using FribergFastigheter.Client.Services.FribergFastigheterApi;
using FribergFastigheter.Shared.Dto;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using static FribergFastigheter.Client.Components.ConfirmButton;

namespace FribergFastigheter.Client.Components
{
    /// <summary>
    /// A component that displays a delete button and handles deletion of a housing object. 
    /// </summary>
    /// <!-- Author: Jimmie -->
    /// <!-- Co Authors: -->
    public partial class DeleteHousing : ComponentBase
    {
        #region Properties

        /// <summary>
        /// The injected auto mapper service. 
        /// </summary>
        [Inject]
#pragma warning disable CS8618 
        private IMapper AutoMapper { get; set; }
#pragma warning restore CS8618 

#pragma warning disable CS8618
        /// <summary>
        /// The injected broker firm API service. 
        /// </summary>
        [Inject]
        private IBrokerFirmApiService BrokerFirmApiService { get; set; }
#pragma warning restore CS8618 

#pragma warning disable CS8618 
        /// <summary>
        /// The housing object that can be deleted. 
        /// </summary>
        [Parameter]
        public HousingViewModel Housing { get; set; }
#pragma warning restore CS8618 

        /// <summary>
        /// Event triggers when the housing object have been deleted.
        /// </summary>
        [Parameter]
        public EventCallback<HousingViewModel> OnHousingDeleted { get; set; }

#pragma warning disable CS8618
        /// <summary>
        /// Injected JavaScript runtime.
        /// </summary>
        [Inject]
        public IJSRuntime JsRuntime { get; set; }
#pragma warning restore CS8618

        #endregion

        #region Methods

        /// <summary>
        /// Event handler for when the user makes a choice in the confirm delete housing dialog box. 
        /// </summary>
        /// <param name="result">The result of the user action.</param>
        /// <returns>A <see cref="Task"/>.</returns>
        private async Task OnConfirmDeleteHousingEventHandler(DialogResults result)
        {
            if (result == DialogResults.UserConfirmed)
            {
                await BrokerFirmApiService.DeleteHousing(Housing.HousingId);
                await OnHousingDeleted.InvokeAsync(Housing);       
            }
        }

        #endregion
    }
}

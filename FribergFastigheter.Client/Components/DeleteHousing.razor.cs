using AutoMapper;
using FribergFastigheter.Client.Models;
using FribergFastigheter.Client.Services.FribergFastigheterApi;
using FribergFastigheter.Shared.Dto;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

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

        /// <summary>
        /// Temporary parameter to store the broker firm ID.
        /// TODO - Replace with identity data. 
        /// </summary>
        [Parameter]
        public int BrokerFirmId { get; set; }

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
        /// Event handler for the delete housing object button. 
        /// </summary>
        /// <returns></returns>
        private async Task OnDeleteHousingButtonClicked()
        {
            await BrokerFirmApiService.DeleteHousing(BrokerFirmId, Housing.HousingId, Housing.Images.Select(x => x.ImageId).ToList());
            await OnHousingDeleted.InvokeAsync(Housing);       
        }

        #endregion
    }
}

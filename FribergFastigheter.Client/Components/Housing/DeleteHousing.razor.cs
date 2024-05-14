using AutoMapper;
using FribergFastigheter.Client.Models.Housing;
using FribergFastigheter.Client.Services.FribergFastigheterApi;
using FribergFastigheter.Shared.Constants;
using FribergFastigheter.Shared.Services.AuthorizationHandlers.Housing.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using static FribergFastigheter.Client.Components.ConfirmButton;

namespace FribergFastigheter.Client.Components.Housing
{
    /// <summary>
    /// A component that displays a delete button and handles deletion of a housing object. 
    /// </summary>
    /// <!-- Author: Jimmie -->
    /// <!-- Co Authors: -->
    public partial class DeleteHousing : ComponentBase
    {
        #region InjectedServiceProperties
#pragma warning disable CS8618

        /// <summary>
        /// The injected authorization service. 
        /// </summary>
        [Inject]
        private IAuthorizationService AuthorizationService { get; set; }

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

#pragma warning restore CS8618
        #endregion

        #region ParameterProperties
#pragma warning disable CS8618

        /// <summary>
        /// The cascaded authentication state task.
        /// </summary>
        [CascadingParameter]

        private Task<AuthenticationState> AuthenticationStateTask { get; set; }

        /// <summary>
        /// The housing object that can be deleted. 
        /// </summary>
        [Parameter]
        public HousingViewModel Housing { get; set; }

        /// <summary>
        /// Event triggers when the housing object have been deleted.
        /// </summary>
        [Parameter]
        public EventCallback<HousingViewModel> OnHousingDeleted { get; set; }

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
                var user = (await AuthenticationStateTask).User;
                var brokerFirmId = int.Parse(user.FindFirst(ApplicationUserClaims.BrokerFirmId)!.Value);
                var brokerId = int.Parse(user.FindFirst(ApplicationUserClaims.BrokerId)!.Value);
                var authorizationData = new DeleteHousingAuthorizationData(existingHousingBrokerFirmId: brokerFirmId, existingHousingBrokerId: brokerId);
                var authorizeResult = await AuthorizationService.AuthorizeAsync(user, authorizationData, ApplicationPolicies.CanDeleteHousingResource);

                if (authorizeResult.Succeeded)
                {
                    await BrokerFirmApiService.DeleteHousing(Housing.HousingId);
                    await OnHousingDeleted.InvokeAsync(Housing);
                }
                else
                {
                    // TODO - show message
                }
            }
        }

        #endregion
    }
}

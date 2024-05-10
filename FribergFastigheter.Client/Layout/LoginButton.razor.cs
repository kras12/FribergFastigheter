using AutoMapper;
using FribergFastigheter.Client.Models.BrokerFirm;
using FribergFastigheter.Client.Services;
using FribergFastigheter.Client.Services.FribergFastigheterApi;
using FribergFastigheter.Shared.Dto.Login;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.IdentityModel.Tokens;
using Microsoft.JSInterop;
using static FribergFastigheter.Client.Components.ConfirmButton;

namespace FribergFastigheter.Client.Layout
{
    /// <summary>
    /// A component that handles both login and logout functionality for brokers.
    /// </summary>
    /// <!-- Author: Jimmie -->
    /// <!-- Co Authors: -->
    public partial class LoginButton : ComponentBase
    {
        #region Fields

        /// <summary>
        /// The id of the modal dialg. 
        /// </summary>
        private readonly string _modalDialogId = Guid.NewGuid().ToString();

        #endregion

        #region Properties

        /// <summary>
        /// The injected Auto Mapper service. 
        /// </summary>
        [Inject]
#pragma warning disable CS8618 
        private IMapper AutoMapper { get; set; }
#pragma warning restore CS8618 

        /// <summary>
        /// The injected authentication state provider. 
        /// </summary>
        [Inject]
#pragma warning disable CS8618 
        private AuthenticationStateProvider AuthenticationStateProvider { get; set; }
#pragma warning restore CS8618 

        /// <summary>
        /// The injected broker firm API service. 
        /// </summary>
        [Inject]
#pragma warning disable CS8618 
        private IBrokerFirmApiService BrokerFirmApiService { get; set; }
#pragma warning restore CS8618 

        /// <summary>
        /// The data binding property for the form. 
        /// </summary>
        [SupplyParameterFromForm]
        public LoginViewModel FormInput { get; set; } = new LoginViewModel();

        /// <summary>
        /// The injected JavaScript runtime. 
        /// </summary>
        [Inject]
#pragma warning disable CS8618 
        private IJSRuntime JSRuntime { get; set; }
#pragma warning restore CS8618 

        /// <summary>
        /// The injected navigation manager. 
        /// </summary>
        [Inject]
#pragma warning disable CS8618 
        private NavigationManager NavigationManager { get; set; }
#pragma warning restore CS8618 

        #endregion

        #region Methods

        /// <summary>
        /// Event handler for when the logout button was clicked. 
        /// </summary>
        /// <returns>A <see cref="Task"/> representing an async operation.</returns>
        private async Task OnLogoutButtonClicked()
        {
            await ((ApiAuthenticationStateProvider)AuthenticationStateProvider).RemoveTokenAsync();
            NavigationManager.NavigateToLogout("/");
        }

        /// <summary>
        /// Event handler for the form submit button. 
        /// </summary>
        /// <returns>A <see cref="Task"/> representing an async operation.</returns>
        private async Task OnValidSubmit()
        {
            await JSRuntime.InvokeVoidAsync("HideBrokerLoginModal", _modalDialogId);
            await BrokerFirmApiService.Login(AutoMapper.Map<LoginDto>(FormInput));
            FormInput = new LoginViewModel();
            NavigationManager.NavigateTo("brokermember");
        }

        #endregion
    }
}

﻿using AutoMapper;
using Blazored.SessionStorage;
using FribergFastigheter.Client.Models;
using FribergFastigheter.Client.Models.BrokerFirm;
using FribergFastigheter.Client.Pages.BrokerFirmMember;
using FribergFastigheter.Client.Services.Authentication;
using FribergFastigheter.Shared.Dto.Login;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.JSInterop;

namespace FribergFastigheter.Client.Layout
{
    /// <summary>
    /// A component that handles both login and logout functionality for brokers.
    /// </summary>
    /// <!-- Author: Jimmie -->
    /// <!-- Co Authors: -->
    public partial class LoginButton : ComponentBase
    {
        #region Constants

        /// <summary>
        /// The key where the component looks for redirect url targets after logging in. 
        /// </summary>
        public const string RedirectUrlStorageKey = "RedirectUrlAfterLoginKey";

        #endregion

        #region Fields

        /// <summary>
        /// A collection of validation errors returned from the API.
        /// </summary>
        private List<string> _apiValidationErrors = new List<string>();

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
        /// The injected broker authentication service. 
        /// </summary>
        [Inject]
#pragma warning disable CS8618 
        private IBrokerAuthenticationService BrokerAuthenticationService { get; set; }
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

        /// <summary>
        /// The injected session storage service.
        /// </summary>
        [Inject]
#pragma warning disable CS8618 
        private ISessionStorageService SessionStorageService { get; set; }
#pragma warning restore CS8618 

        #endregion

        #region Methods

        /// <summary>
        /// Event handler for when the logout button was clicked. 
        /// </summary>
        /// <returns>A <see cref="Task"/> representing an async operation.</returns>
        private async Task OnLogoutButtonClicked()
        {
            await BrokerAuthenticationService.Logout();
            NavigationManager.NavigateToLogout("/");
        }

        /// <summary>
        /// Event handler for the form submit button. 
        /// </summary>
        /// <returns>A <see cref="Task"/> representing an async operation.</returns>
        private async Task OnValidSubmit()
        {
            _apiValidationErrors.Clear();
            await JSRuntime.InvokeVoidAsync("HideBrokerLoginModal", _modalDialogId);
            var response = await BrokerAuthenticationService.Login(AutoMapper.Map<LoginDto>(FormInput));

            if (response.Success)
            {
                FormInput = new LoginViewModel();
                string navigateToUrl = BrokerFirmHomePage.BrokerMemberPageUrl;

                if (await SessionStorageService.ContainKeyAsync(RedirectUrlStorageKey))
                {
                    string redirectToUrl = await SessionStorageService.GetItemAsStringAsync(RedirectUrlStorageKey);
                    await SessionStorageService.RemoveItemAsync(RedirectUrlStorageKey);
                    navigateToUrl = redirectToUrl;
                }

                NavigationManager.NavigateTo(navigateToUrl);
            }
            else
            {
                _apiValidationErrors = response.Errors.Select(x => x.Value).ToList();

                // TODO - Find better solution
                // The identity framework provides us with english messages, so we perform a quick translation.
                for (int i = 0; i < _apiValidationErrors.Count; i++)
                {
                    if (_apiValidationErrors[i].Equals("The UserName field is required.", StringComparison.CurrentCultureIgnoreCase))
                    {
                        _apiValidationErrors[i] = ViewModelBase.EmailValidationErrorMessage;
                    }
                    else if (_apiValidationErrors[i].Equals("The Password field is required.", StringComparison.CurrentCultureIgnoreCase))
                    {
                        _apiValidationErrors[i] = ViewModelBase.PasswordValidationErrorMessage;
                    }
                }

                // The framework needs some time it seems
                await Task.Delay(200);
                await JSRuntime.InvokeVoidAsync("ShowBrokerLoginModal", _modalDialogId);
            }
        }

        #endregion
    }
}

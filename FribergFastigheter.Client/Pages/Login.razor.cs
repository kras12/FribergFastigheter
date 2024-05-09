using AutoMapper;
using FribergFastigheter.Client.Models;
using FribergFastigheter.Client.Services.FribergFastigheterApi;
using FribergFastigheter.Shared.Dto;
using Microsoft.AspNetCore.Components;


namespace FribergFastigheter.Client.Pages
{
    /// <summary>
    /// A page that displays a form and handles user logins.
    /// </summary>
    /// <!-- Author: Jimmie, Marcus -->
    /// <!-- Co Authors: -->
    public partial class Login : ComponentBase
    {
        #region Properties

        /// <summary>
        /// The injected Auto Mapper service. 
        /// </summary>
        [Inject]
#pragma warning disable CS8618 
        private IMapper AutoMapper { get; set; }
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
        /// The injected navigation manager. 
        /// </summary>
        [Inject]
#pragma warning disable CS8618 
        private NavigationManager NavigationManager { get; set; }
#pragma warning restore CS8618         

        #endregion

        #region Methods

        /// <summary>
        /// Event handler for the form submit button. 
        /// </summary>
        /// <returns>A <see cref="Task"/> representing an async operation.</returns>
        private async Task OnValidSubmit()
        {
            await BrokerFirmApiService.Login(AutoMapper.Map<LoginDto>(FormInput));
            NavigationManager.NavigateTo("brokermember");
		}

        #endregion
    }
}

using AutoMapper;
using FribergFastigheter.Client.Models.Broker;
using FribergFastigheter.Client.Models.BrokerFirm;
using FribergFastigheter.Client.Services.FribergFastigheterApi;
using FribergFastigheter.Shared.Constants;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.ComponentModel;

namespace FribergFastigheter.Client.Pages.BrokerFirmMember
{
    /// <summary>
    /// The home page of the broker back office. 
    /// </summary>
    /// <!-- Author: Jimmie -->
    /// <!-- Co Authors: -->
    public partial class BrokerFirmHomePage : ComponentBase
    {
        #region Constants

        public const string BrokerMemberPageUrl = "/brokermember";

        #endregion

        #region Fields

        /// <summary>
        /// The statistics for the broker firm. 
        /// </summary>
        private BrokerFirmStatisticsViewModel? _brokerFirmStatistics = null;

        private string? _scrollToTop = null;

        private string? formId = "EditForm";

        /// <summary>
        /// The id for the logged in broker if any. 
        /// </summary>
        int _loggedInBrokerId;

        /// <summary>
        /// The logged in broker if any. 
        /// </summary>
        BrokerViewModel _broker;

        #endregion

        #region Properties

        [CascadingParameter]
        private Task<AuthenticationState> AuthenticationStateTask { get; set; }

        /// <summary>
        /// The injected auto mapper service.
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
        /// Injected JavaScript runtime.
        /// </summary>
        [Inject]
        public IJSRuntime JSRuntime { get; set; }

        [Parameter]
        public int Id { get; set; }

        public bool? IsEditBrokerFormActive { get; set; } = null;


        #endregion

        #region Methods

        /// <summary>
        /// Method invoked when the component is ready to start, having received its
        /// initial parameters from its parent in the render tree.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing any asynchronous operation.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        protected async override Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            var response = await BrokerFirmApiService.GetBrokerFirmStatistics();

            if (response.Success)
            {
                _brokerFirmStatistics = AutoMapper.Map<BrokerFirmStatisticsViewModel>(response.Value!);
            }
            else
            {
                // TODO - Show message
            }
            
            var state = await AuthenticationStateTask;
            _loggedInBrokerId = int.Parse(state.User.FindFirst(ApplicationUserClaims.BrokerId)!.Value);
            var result = await BrokerFirmApiService.GetBrokerById(_loggedInBrokerId);
            if (result.Success)
            {
              _broker = AutoMapper.Map<BrokerViewModel>(result.Value);
            }
        }

        public async void OpenEditBroker()
        {
            
            IsEditBrokerFormActive = true;
            await ScrollToTop(formId);
        }

        public async void OnBrokerEdited()
        {
            IsEditBrokerFormActive = false;
        }

        public void CloseEditBroker()
        {
            IsEditBrokerFormActive = false;
        }

        public async Task ScrollToTop(string formId)
        {
            _scrollToTop = formId;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (_scrollToTop != null)
            {
                await JSRuntime.InvokeVoidAsync("scrollToElement", _scrollToTop);
                _scrollToTop = null;
            }
        }

        #endregion
    }
}

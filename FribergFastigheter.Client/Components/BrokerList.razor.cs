using AutoMapper;
using FribergFastigheter.Client.Models;
using FribergFastigheter.Client.Services.FribergFastigheterApi;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace FribergFastigheter.Client.Components
{
    /// <summary>
    /// A page for listing brokers.
    /// </summary>
    /// <!-- Author: Marcus -->
    /// <!-- Co Authors: -->
    /// 
    public partial class BrokerList : ComponentBase
    {
        #region InjectedServiceProperties

        /// <summary>
        /// The injected Auto Mapper service. 
        /// </summary>
        [Inject]
        private IMapper Mapper { get; set; }
        /// <summary>
        /// The injected housing API service.
        /// </summary>
        [Inject]
        private IBrokerFirmApiService BrokerFirmApiService { get; set; }

        /// <summary>
        /// Injected JavaScript runtime.
        /// </summary>
        [Inject]
        public IJSRuntime JSRuntime { get; set; }

        #endregion
        #region Properties

        [Parameter]
        public List<BrokerViewModel> Brokers { get; set; }
        [Parameter]
        public BrokerFirmSummaryViewModel BrokerFirm { get; set; }

        public bool? IsNewBrokerFormActive { get; set; } = null;

      
        private BrokerList brokerListComponent;

        private string? _scrollToELementId = null;

        #endregion

        #region Constructors

        public BrokerList()
        {
                
        }

        #endregion

        #region Methods


        public void OpenCreateNewBroker()
        {
            IsNewBrokerFormActive = true;
        }

        public void CloseCreateNewBroker(bool close)
        {
            if(close == true)
            {
                IsNewBrokerFormActive = false;
            }
            
        }

        public async void OnCreatedBroker(BrokerViewModel newBroker) 
        {
            Brokers.Add(newBroker);
            IsNewBrokerFormActive = false;
            await ScrollToElement(newBroker);
        }

        public async Task ScrollToElement(BrokerViewModel newBroker)
        {
            _scrollToELementId = $"BrokerListItem-{newBroker.BrokerId}";
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (_scrollToELementId != null)
            {
                await JSRuntime.InvokeVoidAsync("scrollToElement", _scrollToELementId);
                _scrollToELementId = null;
            }
        }

        #endregion
    }
}

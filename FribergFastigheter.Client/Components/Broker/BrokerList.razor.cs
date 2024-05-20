using FribergFastigheter.Client.Models.Broker;
using FribergFastigheter.Client.Models.BrokerFirm;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace FribergFastigheter.Client.Components.Broker
{
    /// <summary>
    /// A page for listing brokers.
    /// </summary>
    /// <!-- Author: Marcus -->
    /// <!-- Co Authors: -->
    /// 
    public partial class BrokerList : ComponentBase
    {

        #region Fields
        private string? _scrollToELementId = null;

        private string? _scrollToTop = null;

        private string? formId = "EditForm";


        #endregion

        #region InjectedServiceProperties

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
        public BrokerViewModel Broker { get; set; }

        public bool? IsNewBrokerFormActive { get; set; } = null;
        public bool? IsEditBrokerFormActive { get; set; } = null;

        [Parameter]
        public bool EnableEditMode { get; set; } = false;


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

        public async void OpenEditBroker(BrokerViewModel broker)
        {
            Broker = broker;
            IsEditBrokerFormActive = true;
            await ScrollToTop(formId);
        }

        public void CloseEditBroker()
        {
            IsEditBrokerFormActive = false;
        }
        public async void OnCreatedBroker(BrokerViewModel newBroker) 
        {
            Brokers.Add(newBroker);
            IsNewBrokerFormActive = false;
            StateHasChanged();
            await ScrollToElement(newBroker);
        }

        public async Task ScrollToElement(BrokerViewModel newBroker)
        {
            _scrollToELementId = $"BrokerListItem-{newBroker.BrokerId}";
        }

        public async Task ScrollToTop(string formId)
        {
            _scrollToTop = formId;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (_scrollToELementId != null)
            {
                await JSRuntime.InvokeVoidAsync("scrollToElement", _scrollToELementId);
                _scrollToELementId = null;
            }
            if (_scrollToTop != null)
            {
                await JSRuntime.InvokeVoidAsync("scrollToElement", _scrollToTop);
                _scrollToTop = null;
            }
        }

        public async void OnBrokerEdited(BrokerViewModel editedBroker)
        {
            IsEditBrokerFormActive = false;
            StateHasChanged();
            await ScrollToElement(editedBroker);
        }

        public async void OnBrokerDeleted(BrokerViewModel broker)
        {
            Brokers.Remove(broker);
            StateHasChanged();
        }
    }
        #endregion
}

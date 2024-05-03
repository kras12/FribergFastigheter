using AutoMapper;
using FribergFastigheter.Client.Models;
using FribergFastigheter.Client.Services.FribergFastigheterApi;
using FribergFastigheter.Shared.Dto;
using Microsoft.AspNetCore.Components;

namespace FribergFastigheter.Client.Components
{

    /// <summary>
    /// A component that provides functionality to edit a broker object by displaying a form to the user.
    /// </summary>
    /// <!-- Author: Marcus -->
    /// <!-- Co Authors: -->
    public partial class EditBroker : ComponentBase
    {
        #region InjectedServiceProperties

        /// <summary>
        /// The injected Auto Mapper service. 
        /// </summary>
        [Inject]
        private IMapper AutoMapper { get; set; }

        /// <summary>
        /// The injected housing API service.
        /// </summary>

        [Inject]
        private IBrokerFirmApiService BrokerFirmApiService { get; set; }

        #endregion

        #region Properties

        [Parameter]
        public BrokerViewModel Broker { get; set; }
        public int BrokerFirmId { get; set; } = 1;
        [SupplyParameterFromForm]
        private EditBrokerViewModel BrokerInput { get; set; } = null;

        [Parameter]
        public EventCallback<bool> CloseEditBroker { get; set; }

        [Parameter]
        public EventCallback<BrokerViewModel> OnBrokerEdited { get; set; }

        #endregion

        #region Methods

        protected async override Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
        }

        private void CreateEditBrokerModel()
        {
            if (Broker == null)
            {
                throw new ArgumentNullException(nameof(Broker), "The broker object can't be null.");
            }

            BrokerInput = AutoMapper.Map<EditBrokerViewModel>(Broker);
        }

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            CreateEditBrokerModel();
        }

        private async Task OnValidSubmit()
        {
            BrokerInput.BrokerFirm.BrokerFirmId = BrokerFirmId;
            await BrokerFirmApiService.UpdateBroker(Broker.BrokerId, AutoMapper.Map<EditBrokerDto>(BrokerInput));
            AutoMapper.Map(BrokerInput!, Broker);
            await OnBrokerEdited.InvokeAsync(Broker);
            //    var newBroker = await BrokerFirmApiService.CreateBroker(BrokerFirmId, Mapper.Map<CreateBrokerDto>(CreateBrokerInput));
            //    await OnBrokerCreated.InvokeAsync(Mapper.Map<BrokerViewModel>(newBroker));
            //}


            
        }

        private async Task CloseEditForm()
        {
            await CloseEditBroker.InvokeAsync(true);
        }

        #endregion
    }
}

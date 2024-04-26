using AutoMapper;
using FribergFastigheter.Client.Models;
using FribergFastigheter.Client.Services.HousingApi;
using FribergFastigheter.Shared.Dto;
using Microsoft.AspNetCore.Components;

namespace FribergFastigheter.Client.Pages.BrokerFirmMember.Broker
{
    /// <summary>
    /// The broker details page.
    /// </summary>
    public partial class BrokerDetailsPage : ComponentBase
    {
        #region Properties
        [Parameter]
        public int Id { get; set; }
        public int BrokerFirmId { get; set; } = 1;
        BrokerViewModel Broker { get; set; }
        [Inject]
        public IBrokerApiService BrokerApiService { get; set; }
        [Inject]
        public IMapper Mapper { get; set; }

        #endregion
        #region Constructors

        public BrokerDetailsPage()
        {

        }

        #endregion

        #region Methods

        protected override async Task OnInitializedAsync()
        {
            BrokerDto? broker = (await BrokerApiService.GetBrokerById(Id, BrokerFirmId));
            BrokerViewModel result = Mapper.Map<BrokerViewModel>(broker);
            Broker = result;
        }

        #endregion


    }
}

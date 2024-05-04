using AutoMapper;
using FribergFastigheter.Client.Models;
using FribergFastigheter.Client.Services.FribergFastigheterApi;
using FribergFastigheter.Shared.Dto;
using Microsoft.AspNetCore.Components;

namespace FribergFastigheter.Client.Pages.BrokerFirmMember.Broker
{

    /// <summary>
    /// The broker details page.
    /// </summary>
    /// <!-- Author: Marcus -->
    /// <!-- Co Authors: -->
    public partial class BrokerDetailsPage : ComponentBase
    {
        #region Properties
        [Parameter]
        public int Id { get; set; }
        public int BrokerFirmId { get; set; } = 1;
        public BrokerViewModel Broker { get; set; }
        public List<HousingViewModel> Housings { get; set; }
        [Inject]
        public IBrokerFirmApiService BrokerApiService { get; set; }
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
            BrokerDto broker = await BrokerApiService.GetBrokerById(Id, BrokerFirmId);
            BrokerViewModel brokerResult = Mapper.Map<BrokerViewModel>(broker);
            Broker = brokerResult;
            List<HousingDto> housings = await BrokerApiService.GetHousings(brokerFirmId: BrokerFirmId, brokerId: Id, limitImagesPerHousing: 3);
            List<HousingViewModel> housingResult = Mapper.Map<List<HousingViewModel>>(housings);
            Housings = housingResult;
        }

        #endregion


    }
}

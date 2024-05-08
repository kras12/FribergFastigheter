using AutoMapper;
using FribergFastigheter.Client.Models;
using FribergFastigheter.Client.Services.FribergFastigheterApi;
using FribergFastigheter.Shared.Dto;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace FribergFastigheter.Client.Pages.BrokerFirmMember.Firm
{
    /// <summary>
    /// The broker details page.
    /// </summary>
    /// <!-- Author: Marcus -->
    /// <!-- Co Authors: -->
    public partial class BrokerFirmDetailsPage: ComponentBase
    {
        #region Properties
        
        public BrokerFirmViewModel BrokerFirm { get; set;}
        public List<BrokerViewModel> Brokers { get; set; }
        public List<HousingViewModel> Housings { get; set; }
        [Inject]
        public IBrokerFirmApiService BrokerFirmApiService {  get; set; }
        [Inject]
        public IMapper Mapper { get; set; }
        public bool? IsBrokerListorHousingListActive { get; set; } = null;

        #endregion

        #region Constructors

        public BrokerFirmDetailsPage()
        {
            
        }

        #endregion

        #region Methods

        protected override async Task OnInitializedAsync()
        {
            BrokerFirmDto brokerFirm = await BrokerFirmApiService.GetBrokerFirm();
            BrokerFirmViewModel brokerFirmResult = Mapper.Map<BrokerFirmViewModel>(brokerFirm);
            BrokerFirm = brokerFirmResult;

            List<BrokerViewModel> result = (await BrokerFirmApiService.GetBrokers())
                .Select(x => Mapper.Map<BrokerViewModel>(x)).ToList();
            Brokers = result;
            Housings = Mapper.Map<List<HousingViewModel>>(await BrokerFirmApiService.GetHousings(3));
        }

        public void OpenBrokerList()
        {
            IsBrokerListorHousingListActive = true;
        }

        public void OpenHousingList()
        {
            IsBrokerListorHousingListActive = false;
        }
        #endregion
    }
}

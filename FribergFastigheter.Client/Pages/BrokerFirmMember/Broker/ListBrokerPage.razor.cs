using FribergFastigheter.Client.Models;
using FribergFastigheter.Shared.Dto;
using Microsoft.AspNetCore.Components;
using System.Runtime.CompilerServices;
using AutoMapper;
using FribergFastigheter.Client.Services.FribergFastigheterApi;

namespace FribergFastigheter.Client.Pages.BrokerFirmMember.Broker
{
    /// <summary>
    /// Code behind for page listing Brokers by Brokerfirm.
    /// </summary>
    /// <!-- Author: Marcus -->
    /// <!-- Co Authors: -->
    public partial class ListBrokerPage : ComponentBase
    {
        #region Properties

        public BrokerFirmSummaryViewModel BrokerFirm { get; set; }
        public List<BrokerViewModel> Brokers { get; set; } = new List<BrokerViewModel>();
        [Inject]
        public IBrokerFirmApiService BrokerFirmApiService { get; set; }
        [Inject]
        public IMapper Mapper { get; set; }

        #endregion

        #region Fields


        #endregion

        #region Constructors

        public ListBrokerPage()
        {
            
        }
        #endregion

        #region Methods


        protected override async Task OnInitializedAsync()
        { 
            List<BrokerViewModel> result = (await BrokerFirmApiService.GetBrokers())
                .Select(x => Mapper.Map<BrokerViewModel>(x)).ToList();
            Brokers = result;


            BrokerFirm = Mapper.Map<BrokerFirmSummaryViewModel>(await BrokerFirmApiService.GetBrokerFirm());      
        }



        #endregion

    }
}

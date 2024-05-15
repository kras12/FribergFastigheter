using FribergFastigheter.Shared.Dto;
using Microsoft.AspNetCore.Components;
using System.Runtime.CompilerServices;
using AutoMapper;
using FribergFastigheter.Client.Services.FribergFastigheterApi;
using FribergFastigheter.Client.Models.BrokerFirm;
using FribergFastigheter.Client.Models.Broker;

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
            var outerResponse = await BrokerFirmApiService.GetBrokers();

            if (outerResponse.Success)
            {
                Brokers = outerResponse.Value!.Select(x => Mapper.Map<BrokerViewModel>(x)).ToList();
                var response = await BrokerFirmApiService.GetBrokerFirm();

                if (response.Success)
                {
                    BrokerFirm = Mapper.Map<BrokerFirmSummaryViewModel>(response.Value!);
                }
                else
                {
                    // TODO - Create message
                }
            }
            else
            {
                // TODO - Create message
            }
        }

        #endregion
    }
}

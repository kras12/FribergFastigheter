using AutoMapper;
using FribergFastigheter.Client.Models.Broker;
using FribergFastigheter.Client.Models.Housing;
using FribergFastigheter.Client.Services.FribergFastigheterApi;
using FribergFastigheter.Shared.Dto.Broker;
using FribergFastigheter.Shared.Dto.Housing;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

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
            var outerResponse = await BrokerApiService.GetBrokerById(Id);

            if (outerResponse.Success)
            {
                BrokerDto broker = outerResponse.Value!;
                BrokerViewModel brokerResult = Mapper.Map<BrokerViewModel>(broker);
                Broker = brokerResult;

                var innerResponse = await BrokerApiService.GetHousings(brokerId: Id, limitImagesPerHousing: 3);

                if (innerResponse.Success)
                {
                    List<HousingDto> housings = innerResponse.Value!;
                    List<HousingViewModel> housingResult = Mapper.Map<List<HousingViewModel>>(housings);
                    Housings = housingResult;
                }
                else
                {
                    // TODO - show message
                }
            }
            else
            {
                // TODO - show message
            }            
        }

        #endregion
    }
}

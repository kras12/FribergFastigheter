﻿using FribergFastigheter.Client.Models;
using FribergFastigheter.Client.Services.HousingApi;
using FribergFastigheter.Shared.Dto;
using Microsoft.AspNetCore.Components;
using System.Runtime.CompilerServices;
using AutoMapper;

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

        
        public int BrokerFirmId { get; set; } = 3000;
        public List<BrokerViewModel> BrokerList { get; set; } = new List<BrokerViewModel>();
        [Inject]
        public IBrokerApiService BrokerApiService { get; set; }
        [Inject]
        public IMapper Mapper { get; set; }

        #endregion
 

        #region Constructors

        public ListBrokerPage()
        {
            
        }

        #endregion

        #region Methods


        protected override async Task OnInitializedAsync()
        { 
            List<BrokerViewModel> result = (await BrokerApiService.GetBrokers(BrokerFirmId))
                .Select(x => Mapper.Map<BrokerViewModel>(x)).ToList();
            BrokerList = result;
        }

        #endregion

    }
}

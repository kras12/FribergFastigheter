﻿using FribergFastigheter.Client.Models;
using FribergFastigheter.Shared.Dto;
using Microsoft.AspNetCore.Components;
using System.Runtime.CompilerServices;
using AutoMapper;
using FribergFastigheter.Client.Services.FribergFastigheterApi;

namespace FribergFastigheter.Client.Pages.BrokerFirmMember.Broker
{
    /// <summary>
    /// The list brokers page.
    /// </summary>
    public partial class ListBrokerPage : ComponentBase
    {
        #region Properties

        
        public int BrokerFirmId { get; set; } = 1;
        public List<BrokerViewModel> BrokerList { get; set; } = new List<BrokerViewModel>();
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
            List<BrokerViewModel> result = (await BrokerFirmApiService.GetBrokers(BrokerFirmId))
                .Select(x => Mapper.Map<BrokerViewModel>(x)).ToList();
            BrokerList = result;
        }

        #endregion

    }
}

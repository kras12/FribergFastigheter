﻿using AutoMapper;
using FribergFastigheter.Client.Models.BrokerFirm;
using FribergFastigheter.Client.Services.FribergFastigheterApi;
using Microsoft.AspNetCore.Components;
using System.ComponentModel;

namespace FribergFastigheter.Client.Pages.BrokerFirmMember
{
    /// <summary>
    /// The home page of the broker back office. 
    /// </summary>
    /// <!-- Author: Jimmie -->
    /// <!-- Co Authors: -->
    public partial class BrokerFirmHomePage : ComponentBase
    {
        #region Fields

        /// <summary>
        /// The statistics for the broker firm. 
        /// </summary>
        private BrokerFirmStatisticsViewModel? _brokerFirmStatistics = null;

        #endregion

        #region Properties

        /// <summary>
        /// The injected auto mapper service.
        /// </summary>
        [Inject]
#pragma warning disable CS8618 
        private IMapper AutoMapper { get; set; }
#pragma warning restore CS8618 

        /// <summary>
        /// The injected broker firm API service. 
        /// </summary>
        [Inject]
#pragma warning disable CS8618 
        private IBrokerFirmApiService BrokerFirmApiService { get; set; }
#pragma warning restore CS8618 

        [Parameter]
        public int Id { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Method invoked when the component is ready to start, having received its
        /// initial parameters from its parent in the render tree.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing any asynchronous operation.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        protected async override Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            var response = await BrokerFirmApiService.GetBrokerFirmStatistics();

            if (response.Success)
            {
                _brokerFirmStatistics = AutoMapper.Map<BrokerFirmStatisticsViewModel>(response.Value!);
            }
            else
            {
                // TODO - Show message
            }        
        }

        #endregion
    }
}

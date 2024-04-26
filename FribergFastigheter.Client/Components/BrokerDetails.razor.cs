using AutoMapper;
using FribergFastigheter.Client.Models;
using FribergFastigheter.Client.Services.HousingApi;
using FribergFastigheter.Shared.Dto;
using Microsoft.AspNetCore.Components;

namespace FribergFastigheter.Client.Components
{
    public partial class BrokerDetails : ComponentBase
    {
        #region Properties

        [Parameter]
        public BrokerViewModel Broker { get; set; }
        
        #endregion
        #region Constructors

        public BrokerDetails()
        {

        }

        #endregion

        #region Methods

        #endregion
    }
}


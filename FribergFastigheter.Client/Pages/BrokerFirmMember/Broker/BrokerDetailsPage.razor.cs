﻿using Microsoft.AspNetCore.Components;

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

		#endregion
	}
}
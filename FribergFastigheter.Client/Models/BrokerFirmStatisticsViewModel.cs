﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FribergFastigheter.Shared.Dto.Statistics
{
    /// <summary>
    /// A view model class that contains statistics for a broker firm. 
    /// </summary>
    /// <!-- Author: Jimmie -->
    /// <!-- Co Authors: -->
    public class BrokerFirmStatisticsViewModel
    {
        #region Properties

        /// <summary>
        /// The number of brokers that belongs to the firm. 
        /// </summary>
        public int BrokerCount { get; set; }

        /// <summary>
        /// The number of housing objects that belongs to the firm. 
        /// </summary>
        public int HousingCount { get; set; }

        /// <summary>
        /// A collection of statistic items that contains the number of housing objects per housing category.
        /// </summary>
        public List<StatisticItemViewModel> HousingCountPerCategory { get; set; } = new();

        #endregion
    }
}

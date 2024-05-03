using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FribergFastigheter.Shared.Dto.Statistics
{
    /// <summary>
    /// A DTO class that contains statistics for a broker firm. 
    /// </summary>
    /// <!-- Author: Jimmie -->
    /// <!-- Co Authors: -->
    public class BrokerFirmStatisticsDto
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
        public List<StatisticItemDto> HousingCountPerCategory { get; set; } = new();

        #endregion
    }
}

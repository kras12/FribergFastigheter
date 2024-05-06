using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FribergFastigheter.Shared.Dto.Statistics
{
    /// <summary>
    /// A view model class that stores a number under a key.
    /// </summary>
    /// <!-- Author: Jimmie -->
    /// <!-- Co Authors: -->
    public class StatisticItemViewModel
    {
        #region Properties

        /// <summary>
        /// The key for the statistic.
        /// </summary>
        public string Key { get; set; } = "";

        /// <summary>
        /// The value for the statistic.
        /// </summary>
        public int Value { get; set; } 

        #endregion
    }
}

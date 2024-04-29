using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FribergFastigheter.Client.Models
{
    /// <summary>
    /// A view model class that holds pagination data for searches.
    /// </summary>
    /// <!-- Author: Jimmie -->
    /// <!-- Co Authors: -->
    public class PaginationViewModel
    {
        #region Properties

        /// <summary>
        /// The current page. 
        /// </summary>
        public int CurrentPage { get; set; }

        /// <summary>
        /// The number of pages that can be retreived from the search. 
        /// </summary>
        public int NumberOfRequestablePages
        {
            get
            {
                if (PageSize == null)
                {
                    return TotalResults;
                }
                else
                {
                    return (TotalResults / PageSize.Value) + (TotalResults % PageSize.Value != 0 ? 1 : 0);
                }
            }
        }

        /// <summary>
        /// The page limit that was requested for the search.
        /// </summary>
        public int? PageSize { get; set; } = null;

        /// <summary>
        /// The total number of housing objects that was found in the search.
        /// </summary>
        public int TotalResults { get; set; }

        #endregion
    }
}

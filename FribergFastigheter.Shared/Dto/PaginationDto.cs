using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FribergFastigheter.Shared.Dto
{
    /// <summary>
    /// A DTO class that holds pagination data from a housing search.
    /// </summary>
    /// <!-- Author: Jimmie -->
    /// <!-- Co Authors: -->
    public class PaginationDto
    {
        #region Properties

        /// <summary>
        /// The current page. 
        /// </summary>
        public int CurrentPage { get; set; }

        /// <summary>
        /// The page size was requested for the search.
        /// </summary>
        public int PageSize { get; set; } 

        /// <summary>
        /// The total number of housing objects that was found in the search.
        /// </summary>
        public int TotalResults { get; set; }

        #endregion
    }
}

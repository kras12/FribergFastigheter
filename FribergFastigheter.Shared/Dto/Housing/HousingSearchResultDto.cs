using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FribergFastigheter.Shared.Dto.Housing
{
    /// <summary>
    /// A DTO class that represents the result of a housing search.
    /// </summary>
    /// <!-- Author: Jimmie -->
    /// <!-- Co Authors: -->
    public class HousingSearchResultDto
    {
        #region Properties

        /// <summary>
        /// A collection of found housing objects.
        /// </summary>
        public List<HousingDto> Housings { get; set; } = new();

        /// <summary>
        /// Pagination data if the search was limited in terms of search results. 
        /// </summary>
        public PaginationDto? Pagination { get; set; } = null;

        #endregion
    }
}

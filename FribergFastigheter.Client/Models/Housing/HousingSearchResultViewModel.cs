using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FribergFastigheter.Client.Models.Housing
{
    /// <summary>
    /// A view model class that represents the result of a housing search.
    /// </summary>
    /// <!-- Author: Jimmie -->
    /// <!-- Co Authors: -->
    public class HousingSearchResultViewModel : ViewModelBase
    {
        #region Properties

        /// <summary>
        /// A collection of found housing objects.
        /// </summary>
        public List<HousingViewModel> Housings { get; set; } = new();

        /// <summary>
        /// Pagination data if the search was limited in terms of search results. 
        /// </summary>
        public PaginationViewModel? Pagination { get; set; } = null;

        #endregion
    }
}

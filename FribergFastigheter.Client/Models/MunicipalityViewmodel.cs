using System.ComponentModel.DataAnnotations;

namespace FribergFastigheter.Client.Models
{
    /// <summary>
    /// A view model class that represents a municipality.
    /// </summary>
    /// <!-- Author: Jimmie -->
    /// <!-- Co Authors: -->
    public class MunicipalityViewModel
    {
        #region Properties

        /// <summary>
        /// The ID of the municipality.
        /// </summary>
        public int MunicipalityId { get; set; }

        /// <summary>
        /// The name of the municipality.
        /// </summary>
        public string MunicipalityName { get; set; } = "";

        #endregion
    }
}

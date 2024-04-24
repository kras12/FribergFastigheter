using System.ComponentModel.DataAnnotations;

namespace FribergFastigheter.Client.Models
{
    /// <summary>
    /// A view model class that holds data for updating a housing object.
    /// </summary>
    /// <!-- Author: Jimmie -->
    /// <!-- Co Authors: -->
    public class UpdateHousingViewModel : HousingViewModelBase
    {
        #region Properties

        /// <summary>
        /// The ID of the broker associated with the housing object.
        /// </summary>
        public int BrokerId { get; set; }

        /// <summary>
        /// The ID of the broker associated with the housing object.
        /// </summary>
        public int BrokerFirmId { get; set; }

        /// <summary>
        /// The ID of the category associated with the housing object.
        /// </summary>
        public int CategoryId { get; set; }

        /// <summary>
        /// The ID of the housing object.
        /// </summary>
        public int HousingId { get; set; }

        /// <summary>
        /// The ID of the municipality associated with the housing object.
        /// </summary>
        public int MunicipalityId { get; set; }

        #endregion
    }
}

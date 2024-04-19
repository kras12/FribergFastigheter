using System.ComponentModel.DataAnnotations;

namespace FribergFastigheter.Shared.Dto
{
    /// <summary>
    /// A DTO class that holds data for housing creation.
    /// </summary>
    /// <!-- Author: Jimmie -->
    /// <!-- Co Authors: -->
    public class CreateHousingDto : HousingBaseDto
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
        /// The ID of the municipality associated with the housing object.
        /// </summary>
        public int MunicipalityId { get; set; }

        /// <summary>
        /// A collection of new images.
        /// </summary>
        public List<NewImageDto> NewImages { get; set; } = new();

        #endregion
    }
}

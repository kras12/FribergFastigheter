using System.ComponentModel.DataAnnotations;

namespace FribergFastigheter.Shared.Dto
{
    /// <summary>
    /// A DTO class that represents a municipality.
    /// </summary>
    /// <!-- Author: Jimmie -->
    /// <!-- Co Authors: -->
    public class MunicipalityDto
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

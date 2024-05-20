using System.ComponentModel.DataAnnotations;

namespace FribergFastigheter.Client.Models.Housing
{
    /// <summary>
    /// A view model class that represents a municipality.
    /// </summary>
    /// <!-- Author: Jimmie -->
    /// <!-- Co Authors: -->
    public class MunicipalityViewModel : ViewModelBase
    {
        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public MunicipalityViewModel()
        {

        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="municipalityId">The ID of the municipality.</param>
        /// <param name="municipalityName">The name of the municipality</param>
        public MunicipalityViewModel(int municipalityId, string municipalityName)
        {
            MunicipalityId = municipalityId;
            MunicipalityName = municipalityName;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The ID of the municipality.
        /// </summary>
        public int MunicipalityId { get; set; }

        /// <summary>
        /// The name of the municipality.
        /// </summary>
        public string MunicipalityName { get; set; } = "";

        /// <summary>
        /// The default municipality for including all municipalities.
        /// </summary>
        public static MunicipalityViewModel AllMunicipalities { get; } = new MunicipalityViewModel(0, "Alla");

        #endregion
    }
}

using FribergFastigheter.Client.AuthorizationHandlers;
using FribergFastigheter.Client.Models.Broker;
using FribergFastigheter.Client.Models.Image;
using FribergFastigheter.Shared.Dto;

namespace FribergFastigheter.Client.Models.Housing
{
    /// <summary>
    /// A view model class that represents a housing object.
    /// </summary>
    /// <!-- Author: Jimmie -->
    /// <!-- Co Authors: -->
    public class HousingViewModel : HousingViewModelBase, IAuthEditHousing
    {
        #region Properties

        /// <summary>
        /// The broker of the housing object.
        /// </summary>
        public BrokerViewModel Broker { get; set; } = new();

        /// <summary>
        /// The category of the housing object.
        /// </summary>
        public HousingCategoryViewModel Category { get; set; } = new();

        /// <summary>
        /// The ID of the housing object.
        /// </summary>
        public int HousingId { get; set; }

        /// <summary>
        /// The images associated with the housing object.
        /// </summary>
        public List<ImageViewModel> Images { get; set; } = new();

        /// <summary>
        /// The municipality of the housing object.
        /// </summary>
        public MunicipalityViewModel Municipality { get; set; } = new();

        /// <summary>
        /// An optional URL linking to the housing object.
        /// </summary>
        public string? Url { get; set; } = null;

        #endregion

        #region IAuthEditHousing

        /// <summary>
        /// The broker firm ID
        /// </summary>
        int IAuthEditHousing.BrokerFirmId
        {
            get
            {
                return Broker.BrokerFirm.BrokerFirmId;
            }
        }
        
        /// <summary>
        /// The broker ID
        /// </summary>
        int IAuthEditHousing.BrokerId
        {
            get
            {
                return Broker.BrokerId;
            }
        }

        /// <summary>
        /// The housing ID.
        /// </summary>
        int IAuthEditHousing.HousingId
        {
            get
            {
                return HousingId;
            }
        }

        #endregion
    }
}

using FribergFastigheterApi.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace FribergFastigheter.Server.Data.DTO
{
	/// <summary>
	/// A base DTO class that represents a housing object.
	/// </summary>
	/// <!-- Author: Jimmie -->
	/// <!-- Co Authors: -->
	public class HousingDto : HousingBaseDto
	{
        #region Properties

        /// <summary>
        /// The ID of the housing object.
        /// </summary>
        public int HousingId { get; set; }

        /// <summary>
        /// The broker of the housing object.
        /// </summary>
        public BrokerDto Broker { get; set; }

		/// <summary>
		/// The category of the housing object.
		/// </summary>
		public HousingCategoryDto Category { get; set; }

		/// <summary>
		/// The images associated with the housing object.
		/// </summary>
		public List<ImageDto> Images { get; set; } = new();

		/// <summary>
		/// The municipality of the housing object.
		/// </summary>
		public MunicipalityDto Municipality { get; set; }

		#endregion
	}
}

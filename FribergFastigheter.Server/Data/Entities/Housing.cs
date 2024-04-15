using FribergFastigheterApi.Data.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FribergFastigheter.Data.Entities
{
	/// <summary>
	/// An entity class that represents a housing object.
	/// </summary>
	/// <!-- Author: Jimmie -->
	/// <!-- Co Authors: -->
	[Table("Housings")]
    public class Housing
    {
        #region Properties

        /// <summary>
        /// The address of the housing object.
        /// </summary>
        public string Address { get; set; } = "";

        /// <summary>
        /// The ancillaryArea (m2) of the housing object.
        /// </summary>
        public double? AncillaryArea { get; set; }

		/// <summary>
		/// The broker of the housing object.
		/// </summary>
		[Required]
		public Broker Broker { get; set; }

        /// <summary>
        /// The broker firm of the housing object.
        /// </summary>
		[Required]
        public BrokerFirm BrokerFirm { get; set; }

        /// <summary>
        /// The build year of the housing object.
        /// </summary>
        public int? BuildYear { get; set; }

        /// <summary>
        /// The category of the housing object.
        /// </summary>
		
        public HousingCategory Category { get; set; }

        /// <summary>
        /// The description of the housing object.
        /// </summary>
        public string Description { get; set; } = "";

		/// <summary>
		/// The ID of the housing object.
		/// </summary>
		[Key]
		public int HousingId { get; set; }

		/// <summary>
		/// The images associated with the housing object.
		/// </summary>
		public List<Image> Images { get; set; } = new();

		/// <summary>
		/// The land area (m2) of the housing object.
		/// </summary>
		public double? LandArea { get; set; }

		/// <summary>
		/// The living area (m2) of the housing object.
		/// </summary>
		public double LivingArea { get; set; }

		/// <summary>
		/// The monthly fee of the housing object.
		/// </summary>
		public decimal? MonthlyFee { get; set; } = null;

		/// <summary>
		/// The municipality of the housing object.
		/// </summary>
		
		public Municipality Municipality { get; set; }

		/// <summary>
		/// The price of the housing object.
		/// </summary>
		public decimal Price { get; set; }

		/// <summary>
		/// The number of rooms of the housing object.
		/// </summary>
		public int? RoomCount { get; set; } = null;

		/// <summary>
		/// The yearly running cost of the housing object.
		/// </summary>
		public decimal? YearlyRunningCost { get; set; } = null;

        #endregion
    }
}

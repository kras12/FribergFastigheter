using FribergFastigheterApi.HelperClasses.Data;

namespace FribergFastigheter.Server.HelperClasses.Data
{
	/// <summary>
	/// A class that stores grouped seed data for a housing object.
	/// </summary>
	/// <!-- Author: Jimmie -->
	/// <!-- Co Authors: -->
	public class GroupedHousingSeedData
    {
        #region Properties

        /// <summary>
        /// A lookup table for extra housing data.
        /// </summary>
        public Dictionary<string, string> ExtraData { get; set; } = new();

        /// <summary>
        /// A collection of image URLs for the housing object.
        /// </summary>
        public List<string> Images { get; set; } = new();

        /// <summary>
        /// The main data row to get house data from.
        /// </summary>
        public SeedFileDataRow MainDataRow { get; set; } = new();

        /// <summary>
        /// The source url of the seed housing object.
        /// </summary>
        public string PageUrl { get; set; } = "";

        #endregion
    }
}

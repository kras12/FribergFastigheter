namespace FribergFastigheter.Server.HelperClasses.Data
{
    /// <summary>
    /// A class that stores collections of urls for various types of images.
    /// </summary>
    public class ImageUrlCollection
    {
        #region Properties

        /// <summary>
        /// Returns a new collection that combines all other image collections. 
        /// </summary>
        public List<string> AllImageUrls
        {
            get
            {
                var result = new List<string>();
                result.AddRange(BrokerImages);
                result.AddRange(HousingImageUrls);
                result.AddRange(BrokerFirmImages);
                return result;
            }
        }

        /// <summary>
        /// A collection of urls for broker firm images.
        /// </summary>
        public List<string> BrokerFirmImages { get; set; } = new();

        /// <summary>
        /// A collection of urls for broker images.
        /// </summary>
        public List<string> BrokerImages { get; set; } = new();

        /// <summary>
        /// A collection of urls for housing images.
        /// </summary>
        public List<string> HousingImageUrls { get; set; } = new();

        #endregion
    }
}

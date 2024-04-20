using FribergFastigheter.Data.Entities;
using FribergFastigheterApi.Data.Entities;

namespace FribergFastigheter.Server.HelperClasses.Data
{
    /// <summary>
    /// Stores seed data for housings.
    /// </summary>
    /// <!-- Author: Jimmie -->
    /// <!-- Co Authors: -->
    public class HousingSeedData
    {
        #region Properties

        /// <summary>
        /// Returns a list of categories associated with the housings.
        /// </summary>
        public List<HousingCategory> HousingCategories
        {
            get
            {
                return Housings.Select(x => x.Category).ToHashSet().ToList();
            }
        }

        /// <summary>
        /// A collection of housing objects.
        /// </summary>
        public List<Housing> Housings { get; set; } = new();

        /// <summary>
        /// Returns a list of municipalities associated with the housings.
        /// </summary>
        public List<Municipality> Municipalities
        {
            get
            {
                return Housings.Select(x => x.Municipality).ToHashSet().ToList();
            }
        }

        /// <summary>
        /// A collection of urls tied to the housings, brokers, and broker firms. 
        /// </summary>
        public ImageUrlCollection SeedImageUrls { get; set; } = new();

        #endregion
    }
}

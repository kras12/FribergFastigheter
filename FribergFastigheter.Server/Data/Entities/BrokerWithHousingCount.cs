namespace FribergFastigheter.Server.Data.Entities
{
    /// <summary>
	/// Extends the <see cref="Broker"/> entity class with a housing count property. 
	/// </summary>
	/// <!-- Author: Jimmie -->
	/// <!-- Co Authors: -->
    public class BrokerWithHousingCount : Broker
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public BrokerWithHousingCount()
        {
            
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="broker">The broker.</param>
        /// <param name="housingCount">The number of housings that the broker manages. </param>
        public BrokerWithHousingCount(Broker broker, int housingCount) 
            : base(broker.BrokerFirm, broker.Description, broker.ProfileImage, broker.User)
        {
            BrokerId = broker.BrokerId;
            HousingCount = housingCount;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The number of housings that the broker manages. 
        /// </summary>
        public int HousingCount { get; set; }

		#endregion
	}
}

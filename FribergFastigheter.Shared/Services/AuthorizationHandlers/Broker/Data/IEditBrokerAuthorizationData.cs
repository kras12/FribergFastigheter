using FribergFastigheter.Shared.Dto.Broker;

namespace FribergFastigheter.Shared.Services.AuthorizationHandlers.Broker.Data
{
	/// <!-- Author: Jimmie, Marcus -->
	/// <!-- Co Authors: -->
	public interface IEditBrokerAuthorizationData
    {
        /// <summary>
        /// The exsting broker.
        /// </summary>
        public BrokerDto ExistingBroker { get; set; }

        /// <summary>
        /// The new broker. 
        /// </summary>
        public EditBrokerDto NewBroker { get; set; }
    }
}

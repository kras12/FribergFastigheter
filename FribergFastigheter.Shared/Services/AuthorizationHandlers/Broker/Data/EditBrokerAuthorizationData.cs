using FribergFastigheter.Shared.Dto.Broker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FribergFastigheter.Shared.Services.AuthorizationHandlers.Broker.Data
{
    public class EditBrokerAuthorizationData : IEditBrokerAuthorizationData
    {
        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="existingBroker">The existing broker.</param>
        /// <param name="newBroker">The new broker.</param>
        public EditBrokerAuthorizationData(BrokerDto existingBroker, EditBrokerDto newBroker)
        {
            ExistingBroker = existingBroker;
            NewBroker = newBroker;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The exsting broker.
        /// </summary>
        public BrokerDto ExistingBroker { get; set; }

        /// <summary>
        /// The new broker. 
        /// </summary>
        public EditBrokerDto NewBroker { get; set; }

        #endregion
    }
}

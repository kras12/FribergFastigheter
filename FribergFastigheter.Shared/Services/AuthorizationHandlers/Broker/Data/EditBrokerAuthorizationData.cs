using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FribergFastigheter.Shared.Services.AuthorizationHandlers.Broker.Data
{
    public class EditBrokerAuthorizationData : IEditBrokerAuthorizationData
    {
        #region Properties

        /// <summary>
        /// The broker firm ID.
        /// </summary>
        public int ExistingBrokerBrokerFirmId { get; }

        /// <summary>
        /// The broker ID.
        /// </summary>
        public int ExistingBrokerBrokerId { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="existingBrokerBrokerFirmId">The brokerfirm ID.</param>
        /// <param name="existingBrokerBrokerId">The broker ID.</param>
        public EditBrokerAuthorizationData(int existingBrokerBrokerFirmId,
            int existingBrokerBrokerId)
        {
            ExistingBrokerBrokerFirmId = existingBrokerBrokerFirmId;
            ExistingBrokerBrokerId = existingBrokerBrokerId;
        }

        #endregion
    }
}

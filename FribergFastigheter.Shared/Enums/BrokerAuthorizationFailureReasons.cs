using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FribergFastigheter.Shared.Enums
{
    /// <summary>
    /// Various reasons of authorization failure for broker manipulation. 
    /// </summary>
    /// <!-- Author: Jimmie, Marcus -->
    /// <!-- Co Authors: -->
    public enum BrokerAuthorizationFailureReasons
    {
        Unknown,
        BrokerFirmChangeDenied,
        BrokerAccessDenied,
        BrokerCreateAccessDenied,
        BrokerDeleteAccessDenied,
        BrokerEditAccessDenied,
        InvalidToken,
        UnsupportedAction
    }
}

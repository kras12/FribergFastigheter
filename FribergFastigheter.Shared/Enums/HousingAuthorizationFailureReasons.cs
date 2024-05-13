using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FribergFastigheter.Shared.Enums
{
    /// <summary>
    /// Various reasons of authorization failure for housing manipulation. 
    /// </summary>
    /// <!-- Author: Jimmie -->
    /// <!-- Co Authors: -->
    public enum HousingAuthorizationFailureReasons
    {
        Unknown,
        BrokerChangeDenied,
        BrokerFirmChangeDenied,
        HousingAccessDenied,
        HousingCreateAccessDenied,
        HousingDeleteAccessDenied,
        HousingEditAccessDenied,
        InvalidToken,
        UnsupportedAction
    }
}

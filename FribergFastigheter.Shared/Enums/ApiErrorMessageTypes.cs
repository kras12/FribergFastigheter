using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FribergFastigheter.Shared.Enums
{
    /// <summary>
    /// Supported API error message types.
    /// </summary>
    /// <!-- Author: Jimmie -->
    /// <!-- Co Authors: -->
    public enum ApiErrorMessageTypes
    {
        GeneralError,
        AuthorizationError, 
        IdentityError,
        IncompleteInputData,
        InputDataConflict,
        ResourceOwnershipConflict,
        ResourceNotFound,
        ValidationError,        
    }
}

using FribergFastigheter.Shared.Constants;
using FribergFastigheter.Shared.Enums;
using FribergFastigheter.Shared.Services.AuthorizationHandlers.Housing.Data;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics;

namespace FribergFastigheter.Shared.Services.AuthorizationHandlers
{
    /// <summary>
    /// Authorization handler with built in requirement to check associations.
    /// </summary>
    /// <!-- Author: Jimmie -->
    /// <!-- Co Authors: -->
    public class CheckAssociationAuthorizationHandler : AuthorizationHandler<CheckAssociationAuthorizationHandler>, IAuthorizationRequirement
    {
        #region Enums

        /// <summary>
        /// Types of management actions that can be performed. 
        /// </summary>
        public enum ActionTypes
        {
            CheckBrokerFirmAssociation
        }

        #endregion

        #region Fields

        /// <summary>
        /// The action type associated with this instance. 
        /// </summary>
        private readonly ActionTypes _actionType;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="actionType">The action type associated with this instance.</param>
        public CheckAssociationAuthorizationHandler(ActionTypes actionType)
        {
            _actionType = actionType;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Handles the requirement.
        /// </summary>
        /// <param name="context">Contains authorization information.</param>
        /// <param name="requirement">The requirement to handle.</param>
        /// <returns>A <see cref="Task"/> representing an async operation.</returns>
        /// <exception cref="ArgumentException"></exception>
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CheckAssociationAuthorizationHandler requirement)
        {
            if (!context.User.HasClaim(c => c.Type == ApplicationUserClaims.BrokerId) ||
                !context.User.HasClaim(c => c.Type == ApplicationUserClaims.BrokerFirmId) ||
                !context.User.HasClaim(c => c.Type == ApplicationUserClaims.UserRole))
            {
                context.Fail(new AuthorizationFailureReason(requirement, HousingAuthorizationFailureReasons.InvalidToken.ToString()));
                return Task.CompletedTask;
            }

            var brokerFirmId = int.Parse(context.User.FindFirst(ApplicationUserClaims.BrokerFirmId)!.Value);

            switch (_actionType)
            {
                case ActionTypes.CheckBrokerFirmAssociation:

                    var brokerAssociationAuthData = context.Resource as IBrokerFirmAssociationAuthorizationData ??
                        throw new ArgumentException($"This authorization check requires a resource of type '{typeof(IBrokerFirmAssociationAuthorizationData)}'.");

                    // Check broker firm association
                    if (brokerAssociationAuthData.BrokerFirmId != brokerFirmId)
                    {
                        context.Fail(new AuthorizationFailureReason(requirement, HousingAuthorizationFailureReasons.BrokerNotBelongingToFirm.ToString()));
                        return Task.CompletedTask;
                    }
                    
                    else
                    {
                        context.Succeed(requirement);
                        return Task.CompletedTask;
                    }                

                default:
                    context.Fail(new AuthorizationFailureReason(requirement, HousingAuthorizationFailureReasons.UnsupportedAction.ToString()));
                    return Task.CompletedTask;
            }
        }

        #endregion
    }
}

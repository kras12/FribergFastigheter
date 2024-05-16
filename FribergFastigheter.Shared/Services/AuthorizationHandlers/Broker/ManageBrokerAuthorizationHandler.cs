using FribergFastigheter.Shared.Constants;
using FribergFastigheter.Shared.Dto.Broker;
using FribergFastigheter.Shared.Enums;
using FribergFastigheter.Shared.Services.AuthorizationHandlers.Broker.Data;
using Microsoft.AspNetCore.Authorization;

namespace FribergFastigheter.Shared.Services.AuthorizationHandlers.Broker
{
    /// <summary>
    /// Authorization handler with built in requirement to handle authorization for create, delete and edit of housing objects. 
    /// </summary>
    /// <!-- Author: Jimmie, Marcus -->
    /// <!-- Co Authors: -->
    public class ManageBrokerAuthorizationHandler : AuthorizationHandler<ManageBrokerAuthorizationHandler>, IAuthorizationRequirement
    {
        #region Enums

        /// <summary>
        /// Types of management actions that can be performed. 
        /// </summary>
        public enum ActionTypes
        {
            CreateBroker,
            DeleteBroker,
            EditBroker
        }

        #endregion

        #region Fields

        /// <summary>
        /// The action type associated with this instance. 
        /// </summary>
        private readonly ActionTypes _editType;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="editType">The action type associated with this instance.</param>
        public ManageBrokerAuthorizationHandler(ActionTypes editType)
        {
            _editType = editType;
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
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ManageBrokerAuthorizationHandler requirement)
        {
            if (!context.User.HasClaim(c => c.Type == ApplicationUserClaims.BrokerId) ||
                !context.User.HasClaim(c => c.Type == ApplicationUserClaims.BrokerFirmId) ||
                !context.User.HasClaim(c => c.Type == ApplicationUserClaims.UserRole))
            {
                context.Fail(new AuthorizationFailureReason(requirement, BrokerAuthorizationFailureReasons.InvalidToken.ToString()));
                return Task.CompletedTask;
            }

            var brokerFirmId = int.Parse(context.User.FindFirst(ApplicationUserClaims.BrokerFirmId)!.Value);
            var brokerId = int.Parse(context.User.FindFirst(ApplicationUserClaims.BrokerId)!.Value);

            switch (_editType)
            {
                case ActionTypes.CreateBroker:

                    var newBrokerAuthData = context.Resource as ICreateBrokerAuthorizationData ??
                        throw new ArgumentException($"This authorization check requires a resource of type '{typeof(ICreateBrokerAuthorizationData)}'.");

                    // Suitable user role
                    if (!context.User.IsInRole(ApplicationUserRoles.BrokerAdmin))
                    {
                        context.Fail(new AuthorizationFailureReason(requirement, BrokerAuthorizationFailureReasons.BrokerCreateAccessDenied.ToString()));
                        return Task.CompletedTask;
                    }
                    else
                    {
                        context.Succeed(requirement);
                        return Task.CompletedTask;
                    }

                case ActionTypes.DeleteBroker:

                    var deleteBrokerAuthData = context.Resource as IDeleteBrokerAuthorizationData ??
                        throw new ArgumentException($"This authorization check requires a resource of type '{typeof(IDeleteBrokerAuthorizationData)}'.");

                    // Broker belongs to another firm
                    if (deleteBrokerAuthData!.ExistingBrokerBrokerFirmId != brokerFirmId)
                    {
                        context.Fail(new AuthorizationFailureReason(requirement, BrokerAuthorizationFailureReasons.BrokerAccessDenied.ToString()));
                        return Task.CompletedTask;
                    }
                    // Suitable user role
                    else if (!context.User.IsInRole(ApplicationUserRoles.BrokerAdmin))
                    {
                        context.Fail(new AuthorizationFailureReason(requirement, BrokerAuthorizationFailureReasons.BrokerDeleteAccessDenied.ToString()));
                        return Task.CompletedTask;
                    }
                    else
                    {
                        context.Succeed(requirement);
                        return Task.CompletedTask;
                    }

                case ActionTypes.EditBroker:

                    var editBrokerAuthData = context.Resource as IEditBrokerAuthorizationData ??
                        throw new ArgumentException($"This authorization check requires a resource of type '{typeof(IEditBrokerAuthorizationData)}'.");

                    // Broker belongs to another firm
                    if (editBrokerAuthData.ExistingBroker.BrokerFirm.BrokerFirmId != brokerFirmId)
                    {
                        context.Fail(new AuthorizationFailureReason(requirement, BrokerAuthorizationFailureReasons.BrokerAccessDenied.ToString()));
                        return Task.CompletedTask;
                    }
                    // Editing another broker
                    else if (editBrokerAuthData.ExistingBroker.BrokerId != brokerId && !context.User.IsInRole(ApplicationUserRoles.BrokerAdmin))
                    {
                        context.Fail(new AuthorizationFailureReason(requirement, BrokerAuthorizationFailureReasons.BrokerEditAccessDenied.ToString()));
                        return Task.CompletedTask;
                    }
                    // Suitable user role
                    else if (!context.User.IsInRole(ApplicationUserRoles.BrokerAdmin) && !context.User.IsInRole(ApplicationUserRoles.Broker))
                    {
                        context.Fail(new AuthorizationFailureReason(requirement, BrokerAuthorizationFailureReasons.BrokerEditAccessDenied.ToString()));
                        return Task.CompletedTask;
                    }
                    // Protected attributes
                    else if (HasProtectedAttribuesChanged(editBrokerAuthData.ExistingBroker, editBrokerAuthData.NewBroker)
                        && !context.User.IsInRole(ApplicationUserRoles.BrokerAdmin))
                    {
                        context.Fail(new AuthorizationFailureReason(requirement, BrokerAuthorizationFailureReasons.BrokerEditAccessDenied.ToString()));
                        return Task.CompletedTask;
                    }
                    else
                    {
                        context.Succeed(requirement);
                        return Task.CompletedTask;
                    }                

                default:
                    context.Fail(new AuthorizationFailureReason(requirement, BrokerAuthorizationFailureReasons.UnsupportedAction.ToString()));
                    return Task.CompletedTask;
            }
        }

        /// <summary>
        /// Returns true if any protected broker attributes have changed. 
        /// These attributes requires elevated acccess to change. 
        /// </summary>
        /// <param name="existingBroker">The existing broker data.</param>
        /// <param name="newBroker">The new broker data.</param>
        /// <returns>True if any of the protected attributes have changed.</returns>
        private bool HasProtectedAttribuesChanged(BrokerDto existingBroker, EditBrokerDto newBroker)
        {
            if (existingBroker.FirstName != newBroker.FirstName)
            {
                return true;
            }
            else if (existingBroker.LastName != newBroker.LastName)
            {
                return true;
            }
            else if (existingBroker.Email != newBroker.Email)
            {
                return true;
            }
            else if (existingBroker.PhoneNumber != newBroker.PhoneNumber)
            {
                return true;
            }

            return false;
        }

        #endregion
    }
}

using FribergFastigheter.Shared.Constants;
using FribergFastigheter.Shared.Enums;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics;

namespace FribergFastigheter.Server.Services.AuthorizationHandlers
{
    /// <summary>
    /// Authorization handler with built in requirement to handle authorization for create, delete and edit of housing objects. 
    /// </summary>
    /// <!-- Author: Jimmie -->
    /// <!-- Co Authors: -->
    public class ManageHousingAuthorizationHandler : AuthorizationHandler<ManageHousingAuthorizationHandler>, IAuthorizationRequirement
    {
        #region Enums

        /// <summary>
        /// Types of management actions that can be performed. 
        /// </summary>
        public enum ActionTypes
        {
            CreateHousing,
            DeleteHousing,
            EditHousing
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
        public ManageHousingAuthorizationHandler(ActionTypes editType)
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
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ManageHousingAuthorizationHandler requirement)
        {
            if (!context.User.HasClaim(c => c.Type == ApplicationUserClaims.BrokerId) ||
                !context.User.HasClaim(c => c.Type == ApplicationUserClaims.BrokerFirmId) ||
                !context.User.HasClaim(c => c.Type == ApplicationUserClaims.UserRole))
            {
                context.Fail(new AuthorizationFailureReason(requirement, HousingAuthorizationFailureReasons.InvalidToken.ToString()));
                return Task.CompletedTask;
            }

            if (_editType != ActionTypes.CreateHousing && context.Resource is not IHousingAuthorizationData)
            {
                throw new ArgumentException($"This authorization check requires a resource of type '{typeof(IHousingAuthorizationData)}'.");
            }

            var brokerFirmId = int.Parse(context.User.FindFirst(ApplicationUserClaims.BrokerFirmId)!.Value);
            var brokerId = int.Parse(context.User.FindFirst(ApplicationUserClaims.BrokerId)!.Value);
            var authorizationData = context.Resource as IHousingAuthorizationData;

            switch (_editType)
            {
                case ActionTypes.CreateHousing:

                    // Change of broker
                    if (authorizationData!.NewHousingBrokerId != brokerId && !context.User.IsInRole(ApplicationUserRoles.BrokerAdmin))
                    {
                        context.Fail(new AuthorizationFailureReason(requirement, HousingAuthorizationFailureReasons.BrokerChangeDenied.ToString()));
                        return Task.CompletedTask;
                    }
                    // Suitable user role
                    else if (!context.User.IsInRole(ApplicationUserRoles.BrokerAdmin) && !context.User.IsInRole(ApplicationUserRoles.Broker))
                    {
                        context.Fail(new AuthorizationFailureReason(requirement, HousingAuthorizationFailureReasons.HousingCreateAccessDenied.ToString()));
                        return Task.CompletedTask;
                    }
                    else
                    {
                        context.Succeed(requirement);
                        return Task.CompletedTask;
                    }

                case ActionTypes.DeleteHousing:

                    // Housing belongs to another firm
                    if (authorizationData!.ExistingHousingBrokerFirmId != brokerFirmId)
                    {
                        context.Fail(new AuthorizationFailureReason(requirement, HousingAuthorizationFailureReasons.HousingAccessDenied.ToString()));
                        return Task.CompletedTask;
                    }
                    // Housing belongs to another broker
                    else if (authorizationData.ExistingHousingBrokerId != brokerId && !context.User.IsInRole(ApplicationUserRoles.BrokerAdmin))
                    {
                        context.Fail(new AuthorizationFailureReason(requirement, HousingAuthorizationFailureReasons.HousingDeleteAccessDenied.ToString()));
                        return Task.CompletedTask;
                    }
                    // Suitable user role
                    else if (!context.User.IsInRole(ApplicationUserRoles.BrokerAdmin) && !context.User.IsInRole(ApplicationUserRoles.Broker))
                    {
                        context.Fail(new AuthorizationFailureReason(requirement, HousingAuthorizationFailureReasons.HousingDeleteAccessDenied.ToString()));
                        return Task.CompletedTask;
                    }
                    else
                    {
                        context.Succeed(requirement);
                        return Task.CompletedTask;
                    }

                case ActionTypes.EditHousing:

                    // Housing belongs to another firm
                    if (authorizationData!.ExistingHousingBrokerFirmId != brokerFirmId)
                    {
                        context.Fail(new AuthorizationFailureReason(requirement, HousingAuthorizationFailureReasons.HousingAccessDenied.ToString()));
                        return Task.CompletedTask;
                    }             
                    // Change of broker
                    else if (authorizationData.NewHousingBrokerId != authorizationData.ExistingHousingBrokerId && !context.User.IsInRole(ApplicationUserRoles.BrokerAdmin))
                    {
                        context.Fail(new AuthorizationFailureReason(requirement, HousingAuthorizationFailureReasons.BrokerChangeDenied.ToString()));
                        return Task.CompletedTask;
                    }
                    // Housing belongs to another broker
                    else if (authorizationData.ExistingHousingBrokerId != brokerId && !context.User.IsInRole(ApplicationUserRoles.BrokerAdmin))
                    {
                        context.Fail(new AuthorizationFailureReason(requirement, HousingAuthorizationFailureReasons.HousingEditAccessDenied.ToString()));
                        return Task.CompletedTask;
                    }
                    // Suitable user role
                    else if (!context.User.IsInRole(ApplicationUserRoles.BrokerAdmin) && !context.User.IsInRole(ApplicationUserRoles.Broker))
                    {
                        context.Fail(new AuthorizationFailureReason(requirement, HousingAuthorizationFailureReasons.HousingEditAccessDenied.ToString()));
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

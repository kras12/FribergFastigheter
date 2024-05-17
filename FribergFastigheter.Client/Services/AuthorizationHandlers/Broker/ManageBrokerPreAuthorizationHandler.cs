using FribergFastigheter.Client.Services.AuthorizationHandlers.Broker.Data;
using FribergFastigheter.Shared.Constants;
using Microsoft.AspNetCore.Authorization;

namespace FribergFastigheter.Client.Services.AuthorizationHandlers.Broker
{
    /// <summary>
    /// Authorization handler with built in requirement to handle preliminary authorization for create, delete and edit of broker objects. 
    /// </summary>
    /// <!-- Author: Jimmie, Marcus -->
    /// <!-- Co Authors: -->
    public class ManageBrokerPreAuthorizationHandler : AuthorizationHandler<ManageBrokerPreAuthorizationHandler>, IAuthorizationRequirement
    {
        #region Enums

        /// <summary>
        /// Types of management actions that can be performed. 
        /// </summary>
        public enum ActionTypes
        {
            CreateBroker,
            DeleteBroker,
            EditBroker,
            EditFullBroker
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
        public ManageBrokerPreAuthorizationHandler(ActionTypes editType)
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
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ManageBrokerPreAuthorizationHandler requirement)
        {
            if (!context.User.HasClaim(c => c.Type == ApplicationUserClaims.BrokerId) ||
                !context.User.HasClaim(c => c.Type == ApplicationUserClaims.BrokerFirmId) ||
                !context.User.HasClaim(c => c.Type == ApplicationUserClaims.UserRole))
            {
                context.Fail();
                return Task.CompletedTask;
            }

            var brokerFirmId = int.Parse(context.User.FindFirst(ApplicationUserClaims.BrokerFirmId)!.Value);
            var brokerId = int.Parse(context.User.FindFirst(ApplicationUserClaims.BrokerId)!.Value);


            switch (_editType)
            {
                case ActionTypes.CreateBroker:
                    if (context.User.IsInRole(ApplicationUserRoles.BrokerAdmin))
                    {
                        context.Succeed(requirement);
                        return Task.CompletedTask;
                    }
                    break;

                case ActionTypes.DeleteBroker:
                    var deleteAuthorizationData = context.Resource as IDeleteBrokerPreAuthorizationData;

                    if (deleteAuthorizationData == null)
                    {
                        throw new ArgumentException($"This authorization check requires a resource of type '{typeof(IDeleteBrokerPreAuthorizationData)}'.");
                    }

                    if (deleteAuthorizationData.ExistingBrokerBrokerFirmId == brokerFirmId
                         || context.User.IsInRole(ApplicationUserRoles.BrokerAdmin))
                    {
                        context.Succeed(requirement);
                        return Task.CompletedTask;
                    }
                    break;

                case ActionTypes.EditBroker:
                    var editAuthorizationData = context.Resource as IEditBrokerPreAuthorizationData;

                    if (editAuthorizationData == null)
                    {
                        throw new ArgumentException($"This authorization check requires a resource of type '{typeof(IEditBrokerPreAuthorizationData)}'.");
                    }

                    if (editAuthorizationData.ExistingBrokerBrokerFirmId == brokerFirmId
                        && (editAuthorizationData.ExistingBrokerBrokerId == brokerId || context.User.IsInRole(ApplicationUserRoles.BrokerAdmin)))
                    {
                        context.Succeed(requirement);
                        return Task.CompletedTask;
                    }
                    break;

                case ActionTypes.EditFullBroker:
                    var editFullAuthorizationData = context.Resource as IEditBrokerPreAuthorizationData;

                    if (editFullAuthorizationData == null)
                    {
                        throw new ArgumentException($"This authorization check requires a resource of type '{typeof(IEditBrokerPreAuthorizationData)}'.");
                    }

                    if (editFullAuthorizationData.ExistingBrokerBrokerFirmId == brokerFirmId 
                        && context.User.IsInRole(ApplicationUserRoles.BrokerAdmin))
                    {
                        context.Succeed(requirement);
                        return Task.CompletedTask;
                    }
                    break;

                default:
                    break;
            }

            context.Fail();
            return Task.CompletedTask;
        }

        #endregion
    }
}

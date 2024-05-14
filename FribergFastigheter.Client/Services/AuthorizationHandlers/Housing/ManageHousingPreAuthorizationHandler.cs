using FribergFastigheter.Client.Services.AuthorizationHandlers.Housing.Data;
using FribergFastigheter.Shared.Constants;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics;


namespace FribergFastigheter.Client.Services.AuthorizationHandlers.Housing
{
    /// <summary>
    /// Authorization handler with built in requirement to handle preliminary authorization for create, delete and edit of housing objects. 
    /// </summary>
    /// <!-- Author: Jimmie -->
    /// <!-- Co Authors: -->
    public class ManageHousingPreAuthorizationHandler : AuthorizationHandler<ManageHousingPreAuthorizationHandler>, IAuthorizationRequirement
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
        public ManageHousingPreAuthorizationHandler(ActionTypes editType)
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
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ManageHousingPreAuthorizationHandler requirement)
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
                case ActionTypes.CreateHousing:
                    if (context.User.IsInRole(ApplicationUserRoles.BrokerAdmin) || context.User.IsInRole(ApplicationUserRoles.Broker))
                    {
                        context.Succeed(requirement);
                        return Task.CompletedTask;
                    }
                    break;

                case ActionTypes.DeleteHousing:
                    var deleteAuthorizationData = context.Resource as IDeleteHousingPreAuthorizationData;

                    if (deleteAuthorizationData == null)
                    {
                        throw new ArgumentException($"This authorization check requires a resource of type '{typeof(IDeleteHousingPreAuthorizationData)}'.");
                    }

                    if (deleteAuthorizationData.ExistingHousingBrokerFirmId == brokerFirmId
                        && (deleteAuthorizationData.ExistingHousingBrokerId == brokerId || context.User.IsInRole(ApplicationUserRoles.BrokerAdmin)))
                    {
                        context.Succeed(requirement);
                        return Task.CompletedTask;
                    }
                    break;

                case ActionTypes.EditHousing:
                    var editAuthorizationData = context.Resource as IEditHousingPreAuthorizationData;

                    if (editAuthorizationData == null)
                    {
                        throw new ArgumentException($"This authorization check requires a resource of type '{typeof(IEditHousingPreAuthorizationData)}'.");
                    }

                    if (editAuthorizationData != null && editAuthorizationData.ExistingHousingBrokerFirmId == brokerFirmId
                        && (editAuthorizationData.ExistingHousingBrokerId == brokerId || context.User.IsInRole(ApplicationUserRoles.BrokerAdmin)))
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

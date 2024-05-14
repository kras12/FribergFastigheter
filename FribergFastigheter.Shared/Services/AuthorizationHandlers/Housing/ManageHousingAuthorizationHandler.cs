﻿using FribergFastigheter.Shared.Constants;
using FribergFastigheter.Shared.Enums;
using FribergFastigheter.Shared.Services.AuthorizationHandlers.Housing.Data;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics;

namespace FribergFastigheter.Shared.Services.AuthorizationHandlers.Housing
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

            var brokerFirmId = int.Parse(context.User.FindFirst(ApplicationUserClaims.BrokerFirmId)!.Value);
            var brokerId = int.Parse(context.User.FindFirst(ApplicationUserClaims.BrokerId)!.Value);

            switch (_editType)
            {
                case ActionTypes.CreateHousing:

                    var newHousingAuthData = context.Resource as ICreateHousingAuthorizationData ??
                        throw new ArgumentException($"This authorization check requires a resource of type '{typeof(ICreateHousingAuthorizationData)}'.");

                    // Change of broker
                    if (newHousingAuthData!.NewHousingBrokerId != brokerId && !context.User.IsInRole(ApplicationUserRoles.BrokerAdmin))
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

                    var deleteHousingAuthData = context.Resource as IDeleteHousingAuthorizationData ??
                        throw new ArgumentException($"This authorization check requires a resource of type '{typeof(IDeleteHousingAuthorizationData)}'.");

                    // Housing belongs to another firm
                    if (deleteHousingAuthData!.ExistingHousingBrokerFirmId != brokerFirmId)
                    {
                        context.Fail(new AuthorizationFailureReason(requirement, HousingAuthorizationFailureReasons.HousingAccessDenied.ToString()));
                        return Task.CompletedTask;
                    }
                    // Housing belongs to another broker
                    else if (deleteHousingAuthData.ExistingHousingBrokerId != brokerId && !context.User.IsInRole(ApplicationUserRoles.BrokerAdmin))
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

                    var editHousingAuthData = context.Resource as IEditHousingAuthorizationData ??
                        throw new ArgumentException($"This authorization check requires a resource of type '{typeof(IEditHousingAuthorizationData)}'.");

                    // Housing belongs to another firm
                    if (editHousingAuthData!.ExistingHousingBrokerFirmId != brokerFirmId)
                    {
                        context.Fail(new AuthorizationFailureReason(requirement, HousingAuthorizationFailureReasons.HousingAccessDenied.ToString()));
                        return Task.CompletedTask;
                    }
                    // Change of broker
                    else if (editHousingAuthData.NewHousingBrokerId != editHousingAuthData.ExistingHousingBrokerId && !context.User.IsInRole(ApplicationUserRoles.BrokerAdmin))
                    {
                        context.Fail(new AuthorizationFailureReason(requirement, HousingAuthorizationFailureReasons.BrokerChangeDenied.ToString()));
                        return Task.CompletedTask;
                    }
                    // Housing belongs to another broker
                    else if (editHousingAuthData.ExistingHousingBrokerId != brokerId && !context.User.IsInRole(ApplicationUserRoles.BrokerAdmin))
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
﻿using FribergFastigheter.Shared.Constants;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics;

namespace FribergFastigheter.Client.AuthorizationHandlers
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
                context.Fail();
                return Task.CompletedTask;
            }

            if (_editType != ActionTypes.CreateHousing && context.Resource is not IAuthEditHousing)
            {
                throw new ArgumentException($"This authorization check requires a resource of type '{typeof(IAuthEditHousing)}'.");
            }

            var brokerFirmId = int.Parse(context.User.FindFirst(ApplicationUserClaims.BrokerFirmId)!.Value);
            var brokerId = int.Parse(context.User.FindFirst(ApplicationUserClaims.BrokerId)!.Value);
            var housingOwner = context.Resource as IAuthEditHousing;


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
                case ActionTypes.EditHousing:
                    if (housingOwner != null && housingOwner.BrokerFirmId == brokerFirmId
                        && (housingOwner.BrokerId == brokerId || context.User.IsInRole(ApplicationUserRoles.BrokerAdmin)))
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
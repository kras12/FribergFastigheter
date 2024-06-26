﻿using FribergFastigheter.Server.Dto;
using FribergFastigheter.Shared.Dto.Api;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics.CodeAnalysis;

namespace FribergFastigheter.Server.Filters
{
    /// <summary>
    /// Reformats validation problems details from bad requests into an <see cref="ApiErrorResponseDto"/> object.
    /// </summary>
    /// <!-- Author: Jimmie -->
    /// <!-- Co Authors: -->
    public class ReformatValidationProblemAttribute : ActionFilterAttribute
    {
        #region Methods

        /// <inheritdoc/>
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            base.OnResultExecuting(context);

            if (context.Result is BadRequestObjectResult result)
            {
                if (result.Value is ValidationProblemDetails details)
                {
                    var errorMessages = details.Errors.SelectMany(x => x.Value.Select(y => new KeyValuePair<string, string>(x.Key, y))).ToList();
                    context.Result = new BadRequestObjectResult(new MvcApiErrorResponseDto(errors: errorMessages));
                }
            }
        }

        #endregion
    }
}

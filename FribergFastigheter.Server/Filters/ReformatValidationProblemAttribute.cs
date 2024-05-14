using FribergFastigheter.Shared.Dto.Error;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics.CodeAnalysis;

namespace FribergFastigheter.Server.Filters
{
    /// <summary>
    /// Reformats validation problems details from bad requests into an <see cref="ApiErrorResponseDto"/> object.
    /// </summary>
    /// <!-- Author: Jimmie -->
    /// <!-- Co Authors: Marcus-->
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
                    var errorMessages = details.Errors.Select(x => new ApiErrorDto(x.Key, errorMessages: x.Value.ToList())).ToList();

                    context.Result = new BadRequestObjectResult(new ApiErrorResponseDto(System.Net.HttpStatusCode.BadRequest,
                        errors: errorMessages));
                }
            }
        }

        #endregion
    }
}

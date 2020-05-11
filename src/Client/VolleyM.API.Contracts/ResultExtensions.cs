using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using VolleyM.Domain.Contracts;

namespace VolleyM.API.Contracts
{
    public static class ResultExtensions
    {
        public static async Task<IActionResult> ExecuteHandler<TRequest, TResponse, TModel>(
            this IRequestHandler<TRequest, TResponse> handler,
            TRequest request,
            Func<TResponse, TModel> resultConverter)
            where TRequest : IRequest<TResponse>
        {
            var result = await handler.Handle(request);

            return result.Match(
                v => new OkObjectResult(resultConverter(v)),
                ConvertToHttpError);
        }

        private static IActionResult ConvertToHttpError(Error e)
        {
            return e switch
            {
                { Type: ErrorType.NotFound } => (IActionResult)new NotFoundResult(),
                { Type: ErrorType.Conflict } => new ConflictResult(),
                { Type: ErrorType.NotAuthenticated } => new UnauthorizedResult(),
                { Type: ErrorType.NotAuthorized } => new UnauthorizedResult(),
                { Type: ErrorType.FeatureDisabled } => new StatusCodeResult(418),
                ValidationError validationError => CreateValidationErrorResponse(validationError),
                _ => new StatusCodeResult(500),
            };
        }

        private static UnprocessableEntityObjectResult CreateValidationErrorResponse(ValidationError validationError)
        {
            var details = new ValidationProblemDetails(
                validationError.Result
                    .Errors.ToDictionary(
                        e => e.PropertyName,
                        e => new[] { e.ErrorMessage }));

            return new UnprocessableEntityObjectResult(details);
        }
    }
}
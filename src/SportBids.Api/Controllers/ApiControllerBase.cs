using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SportBids.Application.Common.Errors;

namespace SportBids.Api.Controllers;

[ApiController]
[Authorize]
public abstract class ApiControllerBase : ControllerBase
{
    protected IActionResult ProcessError(List<IError> errors)
    {
        if (errors.All(error => error is ValidationError) || errors.All(error => error is UserCreationError))
        {
            var modelStateDictionary = new ModelStateDictionary();
            foreach (var keyValuePair in errors.SelectMany(error => error.Metadata))
            {
                modelStateDictionary.AddModelError(
                    keyValuePair.Key,
                    keyValuePair.Value.ToString() ?? string.Empty);
            }

            return ValidationProblem(modelStateDictionary);
        }

        var firstError = errors[0];

        var statusCode = firstError switch
        {
            ValidationError => StatusCodes.Status400BadRequest,
            BadRequestError => StatusCodes.Status400BadRequest,
            _ => StatusCodes.Status500InternalServerError
        };
        var errorMessage = firstError.Message;
        if (firstError.Metadata.Count > 0)
        {
            errorMessage += firstError.Metadata.Select(metadata => metadata.Value.ToString())
                .Aggregate(" ", (s, s1) => string.Concat(s, " ", s1));
        }

        return Problem(statusCode: statusCode, title: errorMessage);
    }
}

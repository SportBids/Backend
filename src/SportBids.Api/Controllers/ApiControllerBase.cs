using System.Text;
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
                // modelStateDictionary.AddModelError(
                //     keyValuePair.Key,
                //     keyValuePair.Value.ToString() ?? string.Empty);
                var values = keyValuePair.Value as string[] ?? new[]{""};
                foreach (var value in values)
                {
                    modelStateDictionary.AddModelError(keyValuePair.Key, value);
                }
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
        // var errorMessage = firstError.Message;
        // if (firstError.Metadata.Count > 0)
        // {
        //     errorMessage += firstError.Metadata.Select(metadata => metadata.Value.ToString())
        //         .Aggregate(" ", (s, s1) => string.Concat(s, " ", s1));
        // }

        var errorMessage = errors.Select(e =>
        {
            var sb = new StringBuilder();
            sb.Append(e.Message);
            if (e.Metadata.Count > 0)
            {
                sb.Append("(");
                foreach (var mdv in e.Metadata.Values)
                    sb.AppendLine(mdv.ToString());
                sb.AppendLine(")");
            }
            else sb.AppendLine();
            return sb.ToString();
        }).Aggregate((a, b) => string.Concat(a, b));

        return Problem(statusCode: statusCode, title: errorMessage);
    }
}

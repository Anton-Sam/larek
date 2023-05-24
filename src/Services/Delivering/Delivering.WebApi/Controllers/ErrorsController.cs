using BuildingBlocks.Application.Exceptions;
using BuildingBlocks.Domain;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Delivering.WebApi.Controllers;

[ApiExplorerSettings(IgnoreApi = true)]
public class ErrorsController : ControllerBase
{
    [Route("/error")]
    public IActionResult Error()
    {
        Exception? exception = HttpContext.Features
            .Get<IExceptionHandlerFeature>()?.Error;

        var statusCode = exception switch
        {
            NotFoundException => HttpStatusCode.NotFound,
            DomainException => HttpStatusCode.BadRequest,
            _ => HttpStatusCode.InternalServerError
        };

        return Problem(detail: exception?.Message, statusCode: (int)statusCode);
    }
}
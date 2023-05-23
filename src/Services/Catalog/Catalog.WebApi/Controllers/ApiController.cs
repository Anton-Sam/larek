using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.WebApi.Controllers;

[ApiController]
public class ApiController : ControllerBase
{
    protected ISender Sender { get; }
    
    public ApiController(ISender sender)
    {
        Sender = sender;
    }
}

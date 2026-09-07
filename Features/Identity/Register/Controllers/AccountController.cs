using exam_system.Features.Identity.Register.Commands;
using exam_system.Features.Shared;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace exam_system.Features.Identity.Register.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableRateLimiting("Fixed")]
    public class AccountController(IMediator mediator) : ControllerBase
    {
        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterDto model) 
        {
           var result =  await mediator.Send(new RegisterCommand(model.FullName, model.Email, model.Password));
            if(result)
                return Created();
            return Conflict();
        }
    }
}

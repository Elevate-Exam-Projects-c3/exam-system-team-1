using exam_system.Features.Identity.Register.Commands;
using exam_system.Features.Shared;
using exam_system.ModelVM;
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
        public async Task<IActionResult> Register(RegisterModelVM model) 
        {
            var registerDto = new RegisterDto()
            {
                FullName = model.FullName,
                Email = model.Email,
                Password = model.Password
            };
           var result =  await mediator.Send(new RegisterCommand(registerDto.FullName, registerDto.Email, registerDto.Password));

            if(result)
                return Created();
            return Conflict();
        }
    }
}

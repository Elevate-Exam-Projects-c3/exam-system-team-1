using exam_system.Features.Identity.Login.Commands;
using exam_system.Features.Shared;
using exam_system.ModelVM;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;


   

namespace exam_system.Features.Identity.Login.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class LoginController : ControllerBase
    {
        private readonly IMediator _mediator;

        public LoginController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginModelVM model)
        {
            var loginDto = new LoginDto()
            {
                Email = model.Email,
                Password = model.Password
            };
            var result = await _mediator.Send(new LoginCommand(loginDto.Email, loginDto.Password));

            
                return Ok(result);
            
        }
    }
}


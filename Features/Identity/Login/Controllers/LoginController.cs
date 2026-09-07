using exam_system.Features.Identity.Login.Commands;
using exam_system.Features.Shared;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;


   

namespace exam_system.Features.Identity.Login.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly IMediator _mediator;

        public LoginController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDto model)
        {
            var result = await _mediator.Send(new LoginCommand(model.Email, model.Password));

            if (string.IsNullOrEmpty(result))
            {
                return BadRequest("Email or Password is not correct!");
            }
            else if (result == "A User Cann't have more than one role")
            {
                return BadRequest("The user Must have one role only");
            }
            else
            {
                return Ok(result);
            }
        }
    }
}


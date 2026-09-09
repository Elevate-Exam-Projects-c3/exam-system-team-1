using exam_system.Features.Diplomas.AdminCreateDiploma.Commands;
using exam_system.Features.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace exam_system.Features.Diplomas.AdminCreateDiploma.Controllers
{
    [ApiController]
    [Route("api/admin/diplomas")]
    public class CreateDiplomaController : Controller
    {
        private readonly IMediator _mediator;

        public CreateDiplomaController(IMediator mediator) => _mediator = mediator;

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateDiplomaCommand command)
        {
            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, EndpointResponse<Guid>.FromResult(result));
        }
    }
}

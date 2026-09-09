using exam_system.Features.Diplomas.AdminDeleteDiploma.Commands;
using exam_system.Features.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace exam_system.Features.Diplomas.AdminDeleteDiploma.Controllers
{
    [ApiController]
    [Route ("/api/admin/diploma")]
    public class DeleteDiplomaController : Controller
    {
        private readonly IMediator _mediator;

        public DeleteDiplomaController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpDelete ("{id:guid}")]
        public async Task<IActionResult> Delete([FromBody] DeleteDiplomaCommand command)
        {
            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode ,EndpointResponse<Guid>.FromResult(result));
        }
    }
}

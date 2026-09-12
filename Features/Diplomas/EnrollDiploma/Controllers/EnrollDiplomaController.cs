using exam_system.Features.Diplomas.EnrollDiploma.Commands;
using exam_system.Features.Diplomas.EnrollDiploma.ViewModels;
using exam_system.Features.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace exam_system.Features.Diplomas.EnrollDiploma.Controllers
{

    [ApiController]
    [Route("api/diplomas")]
    public class EnrollDiplomaController : Controller
    {
        private readonly IMediator _mediator;
        public EnrollDiplomaController(IMediator mediator) => _mediator = mediator;

        [HttpPost("{diplomaId:guid}/enroll")]
        public async Task<IActionResult> Enroll(Guid diplomaId, [FromBody] EnrollDiplomaViewModel model)
        {
            var command = new EnrollDiplomaCommand
            {
                DiplomaId = diplomaId,
                StudentId = model.StudentId
            };

            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, EndpointResponse<Guid>.FromResult(result));
        }
    }
}

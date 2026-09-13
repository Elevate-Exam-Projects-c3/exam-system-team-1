using Microsoft.AspNetCore.Mvc;
using MediatR;
using exam_system.Features.Diplomas.AdminUpdateDiploma.Commands;
using exam_system.Features.Shared;
using exam_system.Features.Diplomas.AdminUpdateDiploma.ViewModels;

namespace exam_system.Features.Diplomas.AdminUpdateDiploma.Controllers
{
    [ApiController]
    [Route ("api/admin/diplomas")]
    public class UpdateDiplomaController : Controller
    {
        private readonly IMediator _mediatR;

        public UpdateDiplomaController(IMediator mediatR)
        {
            _mediatR = mediatR;
        }
        [HttpPut ("{id :guid}")]
        public async Task<IActionResult> Update (Guid id,[FromBody] UpdateDiplomaViewModel model)
        {
            var command = new UpdateDiplomaCommand
            {
                Id = id,
                title = model.Title,
                description = model.Description
            };
            var result = await _mediatR.Send(command);
            return StatusCode(result.StatusCode, EndpointResponse<Guid>.FromResult(result));
        }
    }
}

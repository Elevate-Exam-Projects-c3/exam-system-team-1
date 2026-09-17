using exam_system.Domain.Entities.Identity;
using exam_system.Features.Diplomas.BrowseDiplomas.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace exam_system.Features.Diplomas.BrowseDiplomas.Controllers
{
    [ApiController]
    [Route("/api/diplomas")]
    public class BrowseDiplomasController : Controller
    {
        private readonly IMediator _mediator;

        public BrowseDiplomasController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        public async Task<IActionResult> GetDiplomas([FromQuery]GetDiplomasQuery getDiplomasQuery)
        {
            var result = await _mediator.Send(getDiplomasQuery);
            return StatusCode(result.StatusCode,result);
        }
    }
}

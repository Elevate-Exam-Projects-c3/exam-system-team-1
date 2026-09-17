using exam_system.Common.Enums;
using exam_system.Domain.Entities.Diplomas;
using exam_system.Features.Diplomas.EnrollDiploma.Commands;
using exam_system.Features.Diplomas.EnrollDiploma.Orchestrators;
using exam_system.Features.Shared;
using exam_system.Persistence.DataAccess;
using MediatR;

namespace exam_system.Features.Diplomas.EnrollDiploma.Handlers
{
    public class EnrollDiplomaCommandHandler : IRequestHandler<EnrollDiplomaCommand, RequestResponse<Guid>>
    {
        private readonly IMediator _mediator;

        public EnrollDiplomaCommandHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<RequestResponse<Guid>> Handle(EnrollDiplomaCommand request, CancellationToken cancellationToken)
        {
            return await _mediator.Send(
              new EnrollDiplomaOrchestrator(request.DiplomaId, request.StudentId), cancellationToken);

        }
    }
}

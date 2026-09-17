using exam_system.Common.Enums;
using exam_system.Domain.Entities.Diplomas;
using exam_system.Features.Diplomas.EnrollDiploma.Commands;
using exam_system.Features.Diplomas.EnrollDiploma.Queries;
using exam_system.Features.Shared;
using exam_system.Persistence.DataAccess;
using MediatR;


namespace exam_system.Features.Diplomas.EnrollDiploma.Orchestrators
{
    public sealed record EnrollDiplomaOrchestrator(Guid DiplomaId, Guid StudentId) : IRequest<RequestResponse<Guid>>;

    public class EnrollDiplomaOrchestratorHandler : IRequestHandler<EnrollDiplomaOrchestrator,RequestResponse<Guid>>
    {
        private readonly IMediator _mediator;

        public EnrollDiplomaOrchestratorHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<RequestResponse<Guid>> Handle(EnrollDiplomaOrchestrator request, CancellationToken cancellationToken)
        {
            var diplomaExists = await _mediator.Send(
                new GetDiplomaExistsQuery(request.DiplomaId), cancellationToken);

            var notEnrolled = await _mediator.Send(
                new GetExistingEnrollmentQuery(request.DiplomaId, request.StudentId), cancellationToken);

            if (!notEnrolled.Success)
                return RequestResponse<Guid>.Fail(ErrorType.Conflict, "Student is already enrolled in this diploma");

           return await _mediator.Send(
                new CreateEnrollmentCommand(request.DiplomaId, request.StudentId), cancellationToken);
        }
    }
}

using exam_system.Domain.Entities.Diplomas;
using exam_system.Features.Diplomas.EnrollDiploma.Queries;
using exam_system.Persistence.DataAccess;
using MediatR;


namespace exam_system.Features.Diplomas.EnrollDiploma.Orchestrators
{
    public record EnrollDiplomaOrchestrator(Guid DiplomaId, Guid StudentId) : IRequest<Guid?>;

    public class EnrollDiplomaOrchestratorHandler : IRequestHandler<EnrollDiplomaOrchestrator, Guid?>
    {
        private readonly IGenericRepository<StudentEnrollment> _enrollmentRepository;
        private readonly IMediator _mediator;

        public EnrollDiplomaOrchestratorHandler(
            IGenericRepository<StudentEnrollment> enrollmentRepository,
            IMediator mediator)
        {
            _enrollmentRepository = enrollmentRepository;
            _mediator = mediator;
        }

        public async Task<Guid?> Handle(EnrollDiplomaOrchestrator request, CancellationToken cancellationToken)
        {
            var isAlreadyEnrolled = await _mediator.Send(
                new GetExistingEnrollmentQuery(request.DiplomaId, request.StudentId), cancellationToken);

            if (isAlreadyEnrolled)
                return null;

            var enrollment = new StudentEnrollment
            {
                DiplomaId = request.DiplomaId,
                StudentId = request.StudentId
            };

            await _enrollmentRepository.AddAsync(enrollment);
            return enrollment.Id;
        }
    }
}

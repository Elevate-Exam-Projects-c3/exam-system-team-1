using exam_system.Domain.Entities.Diplomas;
using exam_system.Features.Diplomas.EnrollDiploma.Queries;
using exam_system.Persistence.DataAccess;
using MediatR;

namespace exam_system.Features.Diplomas.EnrollDiploma.Handlers
{

    public class GetExistingEnrollmentQueryHandler : IRequestHandler<GetExistingEnrollmentQuery, bool>
    {
        private readonly IGenericRepository<StudentEnrollment> _enrollmentRepository;

        public GetExistingEnrollmentQueryHandler(IGenericRepository<StudentEnrollment> enrollmentRepository)
        {
            _enrollmentRepository = enrollmentRepository;
        }

        public async Task<bool> Handle(GetExistingEnrollmentQuery request, CancellationToken cancellationToken)
        {
            var count = await _enrollmentRepository.CountAsync(
                e => e.DiplomaId == request.DiplomaId && e.StudentId == request.StudentId);
            return count > 0;
        }
    }
}

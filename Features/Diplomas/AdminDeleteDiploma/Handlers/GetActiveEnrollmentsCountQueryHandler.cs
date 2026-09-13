using exam_system.Domain.Entities.Diplomas;
using exam_system.Features.Diplomas.AdminDeleteDiploma.Queries;
using exam_system.Persistence.DataAccess;
using MediatR;

namespace exam_system.Features.Diplomas.AdminDeleteDiploma.Handlers
{
    public class GetActiveEnrollmentsCountQueryHandler : IRequestHandler<GetActiveEnrollmentsCountQuery, int>
    {
        private readonly IGenericRepository<StudentEnrollment> _enrollmentRepository;

        public GetActiveEnrollmentsCountQueryHandler(IGenericRepository<StudentEnrollment> enrollmentRepository)
        {
               _enrollmentRepository = enrollmentRepository;
        }
        public Task<int> Handle(GetActiveEnrollmentsCountQuery request, CancellationToken cancellationToken)
        {
            return _enrollmentRepository.CountAsync(e => e.DiplomaId == request.DiplomaId );
        }
    }
}

using exam_system.Common.Enums;
using exam_system.Domain.Entities.Diplomas;
using exam_system.Features.Diplomas.EnrollDiploma.Queries;
using exam_system.Features.Shared;
using exam_system.Persistence.DataAccess;
using MediatR;

namespace exam_system.Features.Diplomas.EnrollDiploma.Handlers
{
    public class GetDiplomaExistsQueryHandler :IRequestHandler<GetExistingEnrollmentQuery, RequestResponse<bool>>
    {
        private readonly IGenericRepository<Diploma> _diplomaRepository;

        public GetDiplomaExistsQueryHandler(IGenericRepository<Diploma> diplomaRepository)
        {
            _diplomaRepository = diplomaRepository;
        }

        public async Task<RequestResponse<bool>> Handle(GetExistingEnrollmentQuery request, CancellationToken cancellationToken)
        {
            var diplomaExists =await _diplomaRepository.CountAsync(d => d.Id == request.DiplomaId) ;
            return diplomaExists >0
                ?  RequestResponse<bool>.Ok(true)
                :  RequestResponse<bool>.Fail(ErrorType.NotFound ,"Diploma not found");
        }
    }
}

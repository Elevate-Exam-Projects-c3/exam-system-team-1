using exam_system.Domain.Entities.Diplomas;
using exam_system.Features.Diplomas.EnrollDiploma.Commands;
using exam_system.Features.Shared;
using exam_system.Persistence.DataAccess;
using MediatR;

namespace exam_system.Features.Diplomas.EnrollDiploma.Handlers
{
    public class CreateEnrollmentCommandHandler: IRequestHandler<EnrollDiplomaCommand, RequestResponse<Guid>>
    {
        private readonly IGenericRepository<StudentEnrollment> _enrollmentRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateEnrollmentCommandHandler(
        IGenericRepository<StudentEnrollment> enrollmentRepository,
        IUnitOfWork unitOfWork)
    {
        _enrollmentRepository = enrollmentRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<RequestResponse<Guid>> Handle(EnrollDiplomaCommand request, CancellationToken cancellationToken)
    {
        var enrollment = new StudentEnrollment
        {
            DiplomaId = request.DiplomaId,
            StudentId = request.StudentId
        };


        await _enrollmentRepository.AddAsync(enrollment);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return RequestResponse<Guid>.Created(enrollment.Id);
    }
}
}

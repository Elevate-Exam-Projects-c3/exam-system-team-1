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
        private readonly IGenericRepository<Diploma> _diplomaRepository;
        private readonly IGenericRepository<StudentEnrollment> _enrollmentRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;

        public EnrollDiplomaCommandHandler(
            IGenericRepository<Diploma> diplomaRepository,
            IGenericRepository<StudentEnrollment> enrollmentRepository,
            IUnitOfWork unitOfWork,
            IMediator mediator)
        {
            _diplomaRepository = diplomaRepository;
            _enrollmentRepository = enrollmentRepository;
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }

        public async Task<RequestResponse<Guid>> Handle(EnrollDiplomaCommand request, CancellationToken cancellationToken)
        {
            var diploma = await _diplomaRepository.GetByIdAsync(request.DiplomaId);
            if (diploma is null)
                return RequestResponse<Guid>.Fail(ErrorType.NotFound, "Diploma not found");

            var isAlreadyEnrolled = await _mediator.Send(
                new EnrollDiplomaOrchestrator(request.DiplomaId, request.StudentId), cancellationToken);
            if (isAlreadyEnrolled is null)
                return RequestResponse<Guid>.Fail(ErrorType.Conflict, "Student is already enrolled in this diploma");

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

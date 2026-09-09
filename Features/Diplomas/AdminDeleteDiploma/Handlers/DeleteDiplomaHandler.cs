using exam_system.Common.Enums;
using exam_system.Domain.Entities.Diplomas;
using exam_system.Features.Diplomas.AdminDeleteDiploma.Commands;
using exam_system.Features.Shared;
using exam_system.Persistence.DataAccess;
using MediatR;

namespace exam_system.Features.Diplomas.AdminDeleteDiploma.Handlers
{
    public class DeleteDiplomaHandler : IRequestHandler<DeleteDiplomaCommand, RequestResponse<Guid>>
    {
        private readonly IGenericRepository<Diploma> _repository;
        private readonly IGenericRepository<StudentEnrollment> _enrollments;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteDiplomaHandler(IGenericRepository<Diploma> repository,IGenericRepository<StudentEnrollment> enrollments, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _enrollments = enrollments;
            _unitOfWork = unitOfWork;
        }

        public async Task<RequestResponse<Guid>> Handle(DeleteDiplomaCommand request, CancellationToken cancellationToken)
        {
            var diploma = await _repository.GetByIdAsync(request.Id);
            if (diploma is null)
            { return RequestResponse<Guid>.Fail(ErrorType.NotFound, "Diploma not found"); }
            var activeEnrollments = await _enrollments.CountAsync(e => e.DiplomaId == request.Id);
            if(activeEnrollments >0)
             return RequestResponse<Guid>.Fail(ErrorType.Conflict, "Diploma can not be deleted as it has Active Enrollments"); 
            await _repository.DeleteAsync(diploma);
           var result =  await _unitOfWork.SaveChangesAsync(cancellationToken);
            return RequestResponse<Guid>.Ok(diploma.Id , "Diploma Deleted successfully");

        }
    }
}

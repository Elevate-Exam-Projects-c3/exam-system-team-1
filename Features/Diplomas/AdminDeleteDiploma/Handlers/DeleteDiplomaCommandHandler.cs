using exam_system.Common.Enums;
using exam_system.Domain.Entities.Diplomas;
using exam_system.Features.Diplomas.AdminDeleteDiploma.Commands;
using exam_system.Features.Diplomas.AdminDeleteDiploma.Orchestrators;
using exam_system.Features.Shared;
using exam_system.Persistence.DataAccess;
using MediatR;

namespace exam_system.Features.Diplomas.AdminDeleteDiploma.Handlers
{
    public class DeleteDiplomaCommandHandler : IRequestHandler<DeleteDiplomaCommand, RequestResponse<Guid>>
    {
        private readonly IGenericRepository<Diploma> _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;

        public DeleteDiplomaCommandHandler(IGenericRepository<Diploma> repository, IUnitOfWork unitOfWork ,IMediator mediator)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }

        public async Task<RequestResponse<Guid>> Handle(DeleteDiplomaCommand request, CancellationToken cancellationToken)
        {
            var diploma = await _repository.GetByIdAsync(request.Id);
            if (diploma is null)
            { return RequestResponse<Guid>.Fail(ErrorType.NotFound, "Diploma not found"); }
            var hasactiveEnrollments = await _mediator.Send(new DeleteDiplomaOrchestrator(request.Id), cancellationToken);
            if (hasactiveEnrollments )
             return RequestResponse<Guid>.Fail(ErrorType.Conflict, "Diploma can not be deleted as it has Active Enrollments"); 
            await _repository.DeleteAsync(diploma);
            var result =  await _unitOfWork.SaveChangesAsync(cancellationToken);
            return RequestResponse<Guid>.Ok(diploma.Id , "Diploma Deleted successfully");

        }
    }
}

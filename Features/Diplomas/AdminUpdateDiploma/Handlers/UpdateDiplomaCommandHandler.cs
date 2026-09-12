using Azure.Core;
using exam_system.Common.Enums;
using exam_system.Domain.Entities.Diplomas;
using exam_system.Features.Diplomas.AdminCreateDiploma.Commands;
using exam_system.Features.Diplomas.AdminUpdateDiploma.Commands;
using exam_system.Features.Shared;
using exam_system.Persistence.DataAccess;
using MediatR;

namespace exam_system.Features.Diplomas.AdminCreateDiploma.Handlers
{
    public class UpdateDiplomaCommandHandler : IRequestHandler<UpdateDiplomaCommand, RequestResponse<Guid>>
    {
        private readonly IGenericRepository<Diploma> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateDiplomaCommandHandler(IGenericRepository<Diploma> repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<RequestResponse<Guid>> Handle(UpdateDiplomaCommand request, CancellationToken cancellationToken)
        {
            var diploma =await _repository.GetByIdAsync(request.Id);
            if (diploma == null)
            { return RequestResponse<Guid>.Fail(ErrorType.NotFound,"Diploma not found"); }
            diploma.Title = request.title;
            diploma.Description = request.description;
            _repository.Update(diploma);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return RequestResponse<Guid>.Ok(diploma.Id, "Diploma updated successfully");

        }
    }
}

using exam_system.Domain.Entities.Diplomas;
using exam_system.Features.Diplomas.AdminDeleteDiploma.Commands;
using exam_system.Features.Diplomas.AdminDeleteDiploma.Queries;
using exam_system.Features.Shared;
using exam_system.Persistence.DataAccess;
using MediatR;

namespace exam_system.Features.Diplomas.AdminDeleteDiploma.Orchestrators
{
   
        public record DeleteDiplomaOrchestrator(Guid Id):IRequest<bool>;
        public class DeleteDiplomaOrchestratorHandler : IRequestHandler<DeleteDiplomaOrchestrator, bool>
        {
            private readonly IMediator _mediator;
            public DeleteDiplomaOrchestratorHandler(IMediator mediator)
            {
                _mediator = mediator;
            }
            public async Task<bool> Handle(DeleteDiplomaOrchestrator request, CancellationToken cancellationToken)
            {
                var activeEnrollmentsCount = await _mediator.Send(new GetActiveEnrollmentsCountQuery(request.Id), cancellationToken);
                return activeEnrollmentsCount > 0;
            }
        }

   
    
}

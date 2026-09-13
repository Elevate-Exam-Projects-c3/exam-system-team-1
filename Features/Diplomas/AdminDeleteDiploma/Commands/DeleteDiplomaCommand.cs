using exam_system.Features.Shared;
using MediatR;

namespace exam_system.Features.Diplomas.AdminDeleteDiploma.Commands
{
    public record DeleteDiplomaCommand : IRequest<RequestResponse<Guid>>
    {
       public Guid Id { get; set; }
    }
}

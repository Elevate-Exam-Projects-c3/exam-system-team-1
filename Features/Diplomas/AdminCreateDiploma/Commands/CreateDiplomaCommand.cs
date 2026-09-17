using MediatR;
using exam_system.Features.Shared;


namespace exam_system.Features.Diplomas.AdminCreateDiploma.Commands
{
    public record CreateDiplomaCommand(string title, string? description) : IRequest<RequestResponse<Guid>>
    {
    }
}

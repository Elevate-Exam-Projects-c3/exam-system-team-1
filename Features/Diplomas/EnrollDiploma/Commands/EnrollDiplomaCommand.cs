using exam_system.Features.Shared;
using MediatR;

namespace exam_system.Features.Diplomas.EnrollDiploma.Commands
{
    public record EnrollDiplomaCommand : IRequest<RequestResponse<Guid>>
    {
        public Guid StudentId { get; set; }
        public Guid DiplomaId { get; set; }
    }
}

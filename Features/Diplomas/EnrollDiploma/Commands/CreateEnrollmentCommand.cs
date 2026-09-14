using exam_system.Features.Shared;
using MediatR;

namespace exam_system.Features.Diplomas.EnrollDiploma.Commands
{
    public sealed record CreateEnrollmentCommand(Guid DiplomaId, Guid StudentId):IRequest<RequestResponse<Guid>>;
}

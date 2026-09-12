using MediatR;

namespace exam_system.Features.Diplomas.EnrollDiploma.Queries
{
    public record GetExistingEnrollmentQuery(Guid DiplomaId, Guid StudentId) : IRequest<bool>;

}

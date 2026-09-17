using MediatR;

namespace exam_system.Features.Diplomas.AdminDeleteDiploma.Queries
{
    public record GetActiveEnrollmentsCountQuery(Guid DiplomaId) : IRequest<int>
    {
    }
}

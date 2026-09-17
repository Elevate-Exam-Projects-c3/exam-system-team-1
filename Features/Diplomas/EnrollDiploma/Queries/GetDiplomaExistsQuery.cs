namespace exam_system.Features.Diplomas.EnrollDiploma.Queries
{
    public sealed record GetDiplomaExistsQuery(Guid DiplomaId) : IRequest<RequestResponse<bool>>;
    {
    }
}

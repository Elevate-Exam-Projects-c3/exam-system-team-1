using MediatR;
using exam_system.Features.Shared;


namespace exam_system.Features.Diplomas.AdminCreateDiploma.Commands
{
    public record CreateDiplomaCommand:IRequest<RequestResponse<Guid>>
    {
        public string title { get; set; }
        public string? description { get; set; }
    }
}

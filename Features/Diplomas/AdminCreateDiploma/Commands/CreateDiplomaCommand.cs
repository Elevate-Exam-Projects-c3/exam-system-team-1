using MediatR;
using exam_system.Features.Shared;


namespace exam_system.Features.Diplomas.AdminCreateDiploma.Commands
{
    public class CreateDiplomaCommand:IRequest<RequestResponse<Guid>>
    {
        public string title { get; set; }
        public string? description { get; set; }
    }
}

using exam_system.Features.Shared;
using MediatR;

namespace exam_system.Features.Diplomas.AdminUpdateDiploma.Commands
{
    public class UpdateDiplomaCommand : IRequest<RequestResponse<Guid>>
    {
        public Guid Id { get; set; }
        public string title { get; set; }=string.Empty;
        public string? description { get; set; }
    }
}

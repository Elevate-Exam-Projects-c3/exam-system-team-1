using exam_system.Features.Diplomas.BrowseDiplomas.DTO;
using exam_system.Features.Shared;
using MediatR;

namespace exam_system.Features.Diplomas.BrowseDiplomas.Queries
{
    public record GetDiplomasQuery :IRequest<RequestResponse<List<DiplomaItemsDTO>>>
    {
        public required Guid StudentId { get; set; }
    }
}

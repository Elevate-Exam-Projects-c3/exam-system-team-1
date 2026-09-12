using exam_system.Features.Shared;
using MediatR;

namespace exam_system.Features.Identity.Login.Commands
{
    public record LoginCommand(string email,string password) : IRequest<RequestResponse<string>>;
    
}

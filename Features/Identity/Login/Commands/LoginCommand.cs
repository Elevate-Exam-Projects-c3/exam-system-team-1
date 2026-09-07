using MediatR;

namespace exam_system.Features.Identity.Login.Commands
{
    public record LoginCommand(string email,string password) : IRequest<string>;
    
}

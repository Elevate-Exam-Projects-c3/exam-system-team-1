using exam_system.Common.Enums;
using exam_system.Domain.Entities.Identity;
using exam_system.Features.Identity.Register.Commands;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace exam_system.Features.Identity.Register.Handlers
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, bool>
    {
        private readonly UserManager<AppUser> _userManger;
        public RegisterCommandHandler(UserManager<AppUser> userManger)
        {
            _userManger = userManger;
        }
        public async Task<bool> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            
           var user = await _userManger.FindByEmailAsync(request.Email);
            if(user == null)
            {
                var appUser = new AppUser()
                {
                    UserName = request.FullName,
                    Email = request.Email,
                    FullName = request.FullName,
                    EmailConfirmed = false,
                    AccountStatus = AccountStatus.Pending
                };
              var result = await _userManger.CreateAsync(appUser, request.Password);
                if (result.Succeeded)
                {
                  var roleResult =   await _userManger.AddToRoleAsync(appUser, "Student");
                    if (roleResult.Succeeded)
                    {
                        
                        //send an otp
                       
                        return true;
                    }

                }
            }
            
            return false;
        }
    }
}

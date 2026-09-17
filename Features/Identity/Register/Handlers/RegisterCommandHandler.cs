using exam_system.Common.Enums;
using exam_system.Domain.Entities.Identity;
using exam_system.Features.Identity.Register.Commands;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;

namespace exam_system.Features.Identity.Register.Handlers
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, bool>
    {
        private readonly UserManager<AppUser> _userManger;
        private readonly IEmailService _emailService;
        public RegisterCommandHandler(UserManager<AppUser> userManger,IEmailService emailService)
        {
            _userManger = userManger;
            _emailService = emailService;
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

                        var otp = RandomNumberGenerator.GetInt32(
                            100000,
                            1000000);

                        // Send OTP
                        await _emailService.SendEmailAsync(
                            appUser.Email!,
                            "Exam System - Verification Code",
                            $"Your OTP is: {otp}");

                        return true;
                    }

                }
            }
            
            return false;
        }
    }
}

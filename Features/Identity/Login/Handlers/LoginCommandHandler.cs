using exam_system.Common.Enums;
using exam_system.Domain.Entities.Identity;
using exam_system.Features.Identity.Login.Commands;
using exam_system.Features.Shared;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Asn1.Cms;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace exam_system.Features.Identity.Login.Handlers
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, string>
    {
        private readonly UserManager<AppUser> _userManger;
        private readonly OptionsPattern _options;
        public LoginCommandHandler(UserManager<AppUser> userManger,OptionsPattern options)
        {
            _userManger = userManger;
            _options = options;
        }
        public async Task<string> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManger.FindByEmailAsync(request.email);

            if (user != null)
            {
              var flag= await _userManger.CheckPasswordAsync(user, request.password);
                if (flag)
                {
                    if (user.EmailConfirmed == true)
                    {
                        if (user.AccountStatus == AccountStatus.Active)
                        {
                            List<Claim> myClaims = new List<Claim>();
                            myClaims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
                            myClaims.Add(new Claim(ClaimTypes.Name, user.FullName));
                            var roles = await _userManger.GetRolesAsync(user);
                            if (roles.Count == 1)
                            {
                                myClaims.Add(new Claim(ClaimTypes.Role, roles[0]));
                            }
                            else
                            {
                                return "A User Cann't have more than one role";
                            }
                            myClaims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
                            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Key));
                            SigningCredentials signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                            JwtSecurityToken myToken = new JwtSecurityToken(
                                audience: _options.Audeience,
                                issuer:_options.Issuer,
                                expires:DateTime.UtcNow.AddMinutes(15),
                                claims:myClaims,
                                signingCredentials:signingCredentials
                                );

                            return new JwtSecurityTokenHandler().WriteToken(myToken);
                        }
                    }
                }
            }
            return string.Empty;
            
        }

    }
}

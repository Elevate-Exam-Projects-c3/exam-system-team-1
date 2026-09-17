using exam_system.Common.Enums;
using exam_system.Domain.Entities.Identity;
using exam_system.Features.Identity.Login.Commands;
using exam_system.Features.Shared;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Asn1.Cms;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace exam_system.Features.Identity.Login.Handlers
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, RequestResponse<string>>
    {
        private readonly UserManager<AppUser> _userManger;
        private readonly IOptions<JwtOptions> _options;
        public LoginCommandHandler(UserManager<AppUser> userManger,IOptions<JwtOptions> options)
        {
            _userManger = userManger;
            _options = options;
        }

        //RequestResponse As Return Type
        public async Task<RequestResponse<string>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManger.FindByEmailAsync(request.email);
            if(user ==null)
            {
                return RequestResponse<string>.Fail("Fail",400,null);
             }
            
             var IsPasswordCorrect = await _userManger.CheckPasswordAsync(user, request.password);
             if(IsPasswordCorrect == false) 
             {
                return RequestResponse<string>.Fail("Fail", 400, null);
             }
              
                    if (user.EmailConfirmed == true && user.AccountStatus == AccountStatus.Active)
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
                                return RequestResponse<string>.Fail("Fail", 400, null);
                }
                            myClaims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
                            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Value.Key));
                            SigningCredentials signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                            JwtSecurityToken myToken = new JwtSecurityToken(
                                audience: _options.Value.Audience,
                                issuer: _options.Value.Issuer,
                                expires: DateTime.UtcNow.AddMinutes(_options.Value.ExpirationInMinutes),
                                claims: myClaims,
                                signingCredentials: signingCredentials
                                );

                            var result = new JwtSecurityTokenHandler().WriteToken(myToken);
                            return RequestResponse<string>.Ok(result,"Success" ,200);
                        
                    
                
            }
            return RequestResponse<string>.Fail("Failed TO Lgoin", 400, null);
        }
    }
}

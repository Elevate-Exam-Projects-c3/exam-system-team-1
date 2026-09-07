using exam_system.Common.Enums;
using Microsoft.AspNetCore.Identity;

namespace exam_system.Domain.Entities.Identity
{
    public class AppUser : IdentityUser
    {
        public string FullName { get; set;  }
        public AccountStatus AccountStatus { get; set; }
    }
}

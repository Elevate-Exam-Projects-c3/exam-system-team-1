using System.ComponentModel.DataAnnotations;

namespace exam_system.Features.Shared
{
    public class RegisterDto
    {
        [Required]
        [Length(2,100)]
        public string FullName { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}

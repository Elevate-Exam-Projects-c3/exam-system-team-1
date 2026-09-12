using System.ComponentModel.DataAnnotations;

namespace exam_system.ModelVM
{
    public class RegisterModelVM
    {
        [Length(2,100)]
        public string FullName { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [DataType(DataType.Password)]  
        public string Password { get; set; }
    }
}

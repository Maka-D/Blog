using System.ComponentModel.DataAnnotations;

namespace Blog.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string FullName { get; set;}
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } 
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}

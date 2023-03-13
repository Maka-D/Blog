using System.ComponentModel.DataAnnotations;

namespace Blog.ViewModels
{
    public class LogInRequestViewModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}

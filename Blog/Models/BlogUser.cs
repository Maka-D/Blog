using Microsoft.AspNetCore.Identity;

namespace Blog.Models
{
    public class BlogUser :IdentityUser
    {
        public string FullName { get; set; }
        public ICollection<BlogPost> Posts { get; set; } = new List<BlogPost>();
    }
}

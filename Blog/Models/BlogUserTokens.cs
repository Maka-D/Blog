using Microsoft.AspNetCore.Identity;

namespace Blog.Models
{
    public class BlogUserTokens :IdentityUserToken<string>
    {
        public DateTime? ExpirationTime { get; set; }   
    }
}

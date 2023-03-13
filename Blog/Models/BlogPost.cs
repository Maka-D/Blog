using System.ComponentModel.DataAnnotations.Schema;

namespace Blog.Models
{
    public class BlogPost
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string Body { get; set; } = "";
        public string Author { get; set; } = "";

        public string UserId { get; set; }
        public BlogUser User { get; set; }
    }
}

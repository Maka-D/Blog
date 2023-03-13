using Blog.Models;

namespace Blog.Data.Repository
{
    public interface IPostRepository
    {
        Task<BlogPost> CreatePostAsync(BlogPost post);
        Task<BlogPost> DeletePostAsync(int id);
        Task<List<BlogPost>> GetAllPostsAsync();
        Task<BlogPost> GetPostByIdAsync(int id);
        Task<List<BlogPost>> GetPostsByUserIdAsync(string userId);
        Task<BlogPost> UpdatePostAsync(BlogPost post);
    }
}
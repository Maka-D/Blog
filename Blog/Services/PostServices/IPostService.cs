using Blog.ViewModels;

namespace Blog.Services.PostService
{
    public interface IPostService
    {
        Task<PostViewModel> CreatePostAsync(PostViewModel post);
        Task<PostViewModel> DeletePostAsync(int id);
        Task<List<PostViewModel>> GetAllPostsAsync();
        Task<PostViewModel> GetPostByIdAsync(int id);
        Task<List<PostViewModel>> GetPostByUserIdAsync(string userId);
        Task<PostViewModel> UpdatePostAsync(PostViewModel post);
    }
}
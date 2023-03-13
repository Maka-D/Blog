using Blog.Models;
using Blog.Services.PostService;
using Blog.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BlogPostController : Controller
    {
        private readonly IPostService _postService;
        public BlogPostController(IPostService postService)
        {
            _postService = postService;
        }
        [HttpGet("id")]
        public async Task<PostViewModel> GetPostById(int id)
        {
            return await _postService.GetPostByIdAsync(id);
        }
        [HttpGet("userId")]
        public async Task<List<PostViewModel>> GetPostsByUserId(string userId)
        {
            return await _postService.GetPostByUserIdAsync(userId);
        }
        [HttpGet]
        public async Task<List<PostViewModel>> GetAllPosts()
        {
            return await _postService.GetAllPostsAsync();
        }
        [HttpPost("Create")]
        public async Task<PostViewModel> CreatePost(PostViewModel post)
        {
            return await _postService.CreatePostAsync(post);
        }
        [HttpPut("Update")]
        public async Task<PostViewModel> UpdatePost(PostViewModel post)
        {
            return await _postService.UpdatePostAsync(post);
        }
        [HttpDelete("Delete")]
        public async Task<PostViewModel> DeletePost(int id)
        {
            return await _postService.DeletePostAsync(id);
        }
    }
}

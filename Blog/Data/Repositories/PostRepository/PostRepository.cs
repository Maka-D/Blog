using Blog.Data.CustomExceptions;
using Blog.Models;
using Microsoft.EntityFrameworkCore;

namespace Blog.Data.Repository
{
    public class PostRepository : IPostRepository
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public PostRepository(AppDbContext context, IHttpContextAccessor contextAccessor)
        {
            _context = context;
            _httpContextAccessor = contextAccessor;
        }
        /// <summary>
        /// gets post id as parameter and returns blog post object if such data exists in database 
        /// if not throws DoesNotExistsException
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="PostDoesNotExistsException"></exception>
        public async Task<BlogPost> GetPostByIdAsync(int id)
        {
            var post = _context.BlogPost.Where(p => p.Id == id).FirstOrDefault();
            if (post is null)
                throw new PostDoesNotExistsException();
            return await Task.FromResult(post);
        }
        /// <summary>
        /// gets user id as parameter and returns all blog posts of this specific user if there's any in database
        /// if not throws DoesNotExistsException
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <exception cref="PostDoesNotExistsException"></exception>
        public async Task<List<BlogPost>> GetPostsByUserIdAsync(string userId)
        {
            var userPosts = _context.BlogPost.Where(p => p.UserId == userId).ToList();
            if (userPosts is null || userPosts.Count == 0)
                throw new PostDoesNotExistsException();
            return await Task.FromResult(userPosts);
        }
        /// <summary>
        /// returns all bolg posts from database
        /// </summary>
        /// <returns></returns>
        public async Task<List<BlogPost>> GetAllPostsAsync()
        {
            var allPost = _context.BlogPost.ToList();
            if (allPost is null || allPost.Count == 0)
                throw new PostDoesNotExistsException();
            return await Task.FromResult(allPost);
        }
        /// <summary>
        /// gets new blog post object as parameter to create and save it in database
        /// </summary>
        /// <param name="post"></param>
        /// <returns></returns>
        public async Task<BlogPost> CreatePostAsync(BlogPost post)
        {
            _context.BlogPost.Add(post);
            await _context.SaveChangesAsync();
            return post;
        }
        /// <summary>
        /// gets blog post as parameter with updated data and saves it in database
        /// </summary>
        /// <param name="post"></param>
        /// <returns></returns>
        public async Task<BlogPost> UpdatePostAsync(BlogPost post)
        {
            await ValidateUserAction(post);   
            _context.BlogPost.Update(post);
            await _context.SaveChangesAsync();
            return post;
        }
        /// <summary>
        /// deletes specific post
        /// </summary>
        /// <param name="post"></param>
        /// <returns></returns>
        public async Task<BlogPost> DeletePostAsync(int id)
        {
            var post = _context.BlogPost.FirstOrDefault(x => x.Id == id);
            await ValidateUserAction(post);
            _context.BlogPost.Remove(post);
            await _context.SaveChangesAsync();
            return post;
        }
        /// <summary>
        /// gets id of specific post and checks if current user has permission
        ///  for specific actions regarding the post
        /// if yes returns post if not throws exception
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="PostDoesNotExistsException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        private Task<BlogPost> ValidateUserAction(BlogPost post)
        {
            var postUser = _context.Users.Where(x => x.Id == post.UserId).FirstOrDefault();
            if (postUser == null)
                throw new PostDoesNotExistsException();
            var currentUser = _httpContextAccessor.HttpContext?.User;
            if (postUser.UserName != currentUser?.Identity?.Name)
                throw new InvalidOperationException("You Don't Have Permission For This Action!");
            return Task.FromResult(post);
        }

    }
}

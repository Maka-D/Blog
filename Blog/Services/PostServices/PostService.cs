using AutoMapper;
using Blog.Data.Repository;
using Blog.Models;
using Blog.ViewModels;

namespace Blog.Services.PostService
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _repository;
        private readonly IMapper _mapper;

        public PostService(IPostRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<PostViewModel> GetPostByIdAsync(int id)
        {
            return _mapper.Map<PostViewModel>(await _repository.GetPostByIdAsync(id));
        }

        public async Task<List<PostViewModel>> GetPostByUserIdAsync(string userId)
        {
            if (userId == null)
                throw new ArgumentNullException(nameof(userId));
            return _mapper.Map<List<PostViewModel>>(await _repository.GetPostsByUserIdAsync(userId));
        }

        public async Task<List<PostViewModel>> GetAllPostsAsync()
        {
            return _mapper.Map<List<PostViewModel>>(await _repository.GetAllPostsAsync());
        }

        public async Task<PostViewModel> CreatePostAsync(PostViewModel post)
        {
            if (post == null)
                throw new ArgumentNullException(nameof(post));
            var result = await _repository.CreatePostAsync(_mapper.Map<BlogPost>(post));
            return _mapper.Map<PostViewModel>(result);
        }

        public async Task<PostViewModel> UpdatePostAsync(PostViewModel post)
        {
            if (post == null)
                throw new ArgumentNullException(nameof(post));
            var blogPost = _mapper.Map<BlogPost>(post);
            var result = await _repository.UpdatePostAsync(blogPost);
            return _mapper.Map<PostViewModel>(result);
        }

        public async Task<PostViewModel> DeletePostAsync(int id)
        {
            var result = await _repository.DeletePostAsync(id);
            return _mapper.Map<PostViewModel>(result);
        }
    }
}

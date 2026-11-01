using FakeBloggingSystem.DTOs;

namespace FakeBloggingSystem.Interfaces
    {
    public interface IPostService
        {
        Task<PostResponse?> GetPostByIdAsync(int postId, bool includeAuthor = false);


        Task<PostResponse?> CreatePostAsync(PostRequest request);
        }
    }

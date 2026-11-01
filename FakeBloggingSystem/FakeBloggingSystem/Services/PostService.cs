using FakeBloggingSystem.Data;
using FakeBloggingSystem.DTOs;
using FakeBloggingSystem.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FakeBloggingSystem.Services
    {
    public class PostService : IPostService
        {
        private readonly BlogDbContext _dbContext;

        public PostService(BlogDbContext dbContext)
            {
            _dbContext = dbContext;
            }
        public async Task<PostResponse?> CreatePostAsync(PostRequest request)
            {
            var isAuthorExists = await _dbContext.Authors
                            .AnyAsync(a => a.Id == request.AuthorId);

            if (!isAuthorExists)
                {
                return null;
                }

            var post = new Models.Post
                {
                AuthorId = request.AuthorId,
                Title = request.Title,

                Description = request.Description,
                Content = request.Content
                };
            _dbContext.Posts.Add(post);
            await _dbContext.SaveChangesAsync();

            return new PostResponse
                {
                Id = post.Id,
                Title = post.Title,
                Description = post.Description,
                Content = post.Content
                };
            }

        public async Task<PostResponse?> GetPostByIdAsync(int postId, bool includeAuthor = false)
            {
                return await _dbContext.Posts
                    .AsNoTracking()
                    .Where(p => p.Id == postId)
                    .Select(p => new PostResponse
                        {
                        Id = p.Id,
                        Title = p.Title,
                        Description = p.Description,
                        Content = p.Content,
                        Author = includeAuthor ? new AuthorResponse
                            {
                            Id = p.Author.Id,
                            Name = p.Author.Name
                            } : null
                        })
                    .FirstOrDefaultAsync();
            }
        }
    }

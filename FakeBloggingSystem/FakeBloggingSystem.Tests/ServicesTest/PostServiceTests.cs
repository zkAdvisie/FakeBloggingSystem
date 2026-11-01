using FakeBloggingSystem.Data;
using FakeBloggingSystem.DTOs;
using FakeBloggingSystem.Models;
using FakeBloggingSystem.Services;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace FakeBloggingSystem.Tests.ServicesTest
    {
    public class PostServiceTests
        {
        private static async Task<(BlogDbContext Context,
            SqliteConnection Connection)> CreateDbAsync()
            {
            var con = new SqliteConnection("Data Source=:memory:");
            await con.OpenAsync();

            var opts = new DbContextOptionsBuilder<BlogDbContext>()
                .UseSqlite(con)
                .Options;

            var ctx = new BlogDbContext(opts);
            await ctx.Database.EnsureCreatedAsync();

            return (ctx, con);
            }

        [Fact]
        public async Task Create_Post_Succeed()
            {
            var (ctx, conn) = await CreateDbAsync();
            await using var db = ctx;
            await using var connection = conn;

            var author = new Author { Name = "Peter", Surname = "Kelvi" };
            db.Authors.Add(author);
            await db.SaveChangesAsync();

            var service = new PostService(db);
            var req = new PostRequest
                {
                AuthorId = author.Id,
                Title = "Test Post Title",
                Description = "Description",
                Content = "Content"
                };

            var result = await service.CreatePostAsync(req);

            Assert.NotNull(result);
            Assert.True(result.Id != 0);

            var post = await db.Posts.FindAsync(result.Id);
            Assert.NotNull(post);
            Assert.Equal(req.Description, post.Description);

            }

        [Fact]
        public async Task Create_Post_InvalidAuthor_ReturnsNull()
            {
            var (ctx, conn) = await CreateDbAsync();
            await using var db = ctx;
            await using var connection = conn;

          
            var service = new PostService(db);
            var req = new PostRequest
                {
                AuthorId = 123,
                Title = "Test Post Title",
                Description = "Description",
                Content = "Content"
                };

            var result = await service.CreatePostAsync(req);

            Assert.Null(result);
          
            Assert.True(await db.Posts.CountAsync() == 0);
            }

        [Fact]
        public async Task GetPostById_Succeed()
            {
            var (ctx, conn) = await CreateDbAsync();
            await using var db = ctx;
            await using var connection = conn;

            var author = new Author { Name = "Peter", Surname = "Kelvi" };
            db.Authors.Add(author);

            var post = new Post {
                Author = author,
                Title = "Test Post Title",
                Description = "Description",
                Content = "Content"
                };
            db.Posts.Add(post);
            await db.SaveChangesAsync();

            var service = new PostService(db);         

            var result = await service.GetPostByIdAsync(post.Id, false);

            Assert.NotNull(result);
            Assert.Equal(post.Description, result.Description);
            }

        [Fact]
        public async Task GetPostById_NonExistingPost_ReturnsNull()
            {
            var (ctx, conn) = await CreateDbAsync();
            await using var db = ctx;
            await using var connection = conn;

            

            var service = new PostService(db);

            var result = await service.GetPostByIdAsync(123, false);

            Assert.Null(result);
            }

        [Fact]
        public async Task GetPostById_IncludeAuthorInfo_Succeed()
            {
            var (ctx, conn) = await CreateDbAsync();
            await using var db = ctx;
            await using var connection = conn;

            var author = new Author { Name = "Peter", Surname = "Kelvi" };
            db.Authors.Add(author);

            var post = new Post
                {
                Author = author,
                Title = "Test Post Title",
                Description = "Description",
                Content = "Content"
                };
            db.Posts.Add(post);
            await db.SaveChangesAsync();
                    var service = new PostService(db);

            var result = await service.GetPostByIdAsync(post.Id, true);

            Assert.NotNull(result);
            Assert.NotNull(result.Author);
            Assert.Equal(author.Id, result.Author.Id);
            }
        }
    }

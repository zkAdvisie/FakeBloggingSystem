using FakeBloggingSystem.Data;
using FakeBloggingSystem.DTOs;
using FakeBloggingSystem.Models;
using FakeBloggingSystem.Tests.IntegrationTests.Factory;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace FakeBloggingSystem.Tests.IntegrationTests
    {
    public class PostApiIntTests
        {
        [Fact]
        public async Task GetPostById_NotFound()
            {
            using var factory = new
                BlogWebApiFactory();

            var client = factory.CreateClient();

            var resp = await client.GetAsync("/post/111");

            Assert.Equal(HttpStatusCode.NotFound, resp.StatusCode);
            }

        [Fact]
        public async Task CreatePost_ValidRequest_ReturnsCreated()
            {
            await using var factory = new
                BlogWebApiFactory();

            var authorId = 0;
            var client = factory.CreateClient();
            using(var scope = factory.Services.CreateScope())
                {
                var dbContext = scope.ServiceProvider.GetRequiredService<BlogDbContext>();
                var author = new Author
                    {
                    Name = "John Doe",
                   Surname = "Smith"
                    };
                dbContext.Authors.Add(author);
                await dbContext.SaveChangesAsync();
                authorId = author.Id;
                }

                var request = new PostRequest { AuthorId = authorId, Title = "Test Post", Description = "Test Description", Content = "Test Content" };

            //Act
            var response = await client.PostAsJsonAsync("/post", request);
            //Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            var createdPost = await response.Content.ReadFromJsonAsync<Post>();
            Assert.NotNull(createdPost);
            Assert.True(createdPost.Id > 0);
            Assert.NotNull(response.Headers.Location);
            Assert.Contains($"/post/{createdPost.Id}", response.Headers.Location.ToString());
            }

        [Fact]
        public async Task CreatePost_InvalidAuthor_ReturnsBadRequest()
            {
            await using var factory = new
                BlogWebApiFactory();

            var authorId = 0;
            var client = factory.CreateClient();
            
            var request = new PostRequest { AuthorId = authorId, Title = "Test Post", Description = "Test Description", Content = "Test Content" };

            //Act
            var response = await client.PostAsJsonAsync("/post", request);
            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
                        }

        [Fact]
        public async Task CreatePost_InValidTitle_ReturnsBadRequest()
            {
            await using var factory = new
                BlogWebApiFactory();

            var authorId = 0;
            var client = factory.CreateClient();
            
            var request = new PostRequest 
                { AuthorId = authorId, Title = "", Description = "Test Description", Content = "Test Content" };

            //Act
            var response = await client.PostAsJsonAsync("/post", request);
            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
                        }

        [Fact]
        public async Task GetPost_ExistingPost_ReturnsPost()
            {
            await using var factory = new
                BlogWebApiFactory();

            var postId = 0;
            var client = factory.CreateClient();
            using (var scope = factory.Services.CreateScope())
                {
                var dbContext = scope.ServiceProvider.GetRequiredService<BlogDbContext>();
                var author = new Author
                    {
                    Name = "John Doe",
                    Surname = "Smith"
                    };
                var post = new Post
                    {
                    Title = "Test Post",
                    Description = "Test Description",
                    Content = "Test Content",
                    Author = author
                    };
                dbContext.Posts.Add(post);
                await dbContext.SaveChangesAsync();
                postId = post.Id;
                }

            //Act
            var response = await client.GetAsync($"/post/{postId}");
            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var postReceived = await response.Content.ReadFromJsonAsync<Post>();
            Assert.NotNull(postReceived);
            Assert.Equal(postId, postReceived.Id);
            Assert.Null(postReceived.Author);

            //JsonAssertion
            var json = await response.Content.ReadAsStringAsync();
            using var document = JsonDocument.Parse(json);
            Assert.False
                (document.RootElement.TryGetProperty("author", out _));
            }

        [Fact]
        public async Task GetPost_ExistingPostWithAuthor_ReturnsPost()
            {
            await using var factory = new
                BlogWebApiFactory();

            var postId = 0;
            var authorId = 0;
            var client = factory.CreateClient();
            using (var scope = factory.Services.CreateScope())
                {
                var dbContext = scope.ServiceProvider.GetRequiredService<BlogDbContext>();
                var author = new Author
                    {
                    Name = "John Doe",
                    Surname = "Smith"
                    };
                var post = new Post
                    {
                    Title = "Test Post",
                    Description = "Test Description",
                    Content = "Test Content",
                    Author = author
                    };
                dbContext.Posts.Add(post);
                await dbContext.SaveChangesAsync();
                postId = post.Id;
                authorId = author.Id;
                }

            //Act
            var response = await client.GetAsync($"/post/{postId}?includeAuthor=true");
            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var postReceived = await response.Content.ReadFromJsonAsync<Post>();
            Assert.NotNull(postReceived);
            Assert.Equal(postId, postReceived.Id);
            Assert.NotNull(postReceived.Author);
            }
        }
    }

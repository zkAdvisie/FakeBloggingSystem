using FakeBloggingSystem.Controllers;
using FakeBloggingSystem.DTOs;
using FakeBloggingSystem.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace FakeBloggingSystem.Tests.ControllersTest
    {
    public class PostsControllerTests
        {
        private readonly Mock<IPostService> _mockPostService = new();
        private readonly PostsController _controller;

        public PostsControllerTests()
            {
            _controller = new PostsController(_mockPostService.Object);
            }


        [Fact]
        public async Task Create_Post_Returns_Created_Result()
            {
            // Arrange

            var request = new FakeBloggingSystem.DTOs.PostRequest
                {
                AuthorId = 1,
                Title = "Test Post",
                Description = "Test Description",
                Content = "Test Content"
                };

            var response = new FakeBloggingSystem.DTOs.PostResponse
                {
                Id = 1,
                Title = request.Title,
                Description = request.Description,
                Content = request.Content
                };
            _mockPostService.Setup
                (service =>
                service.CreatePostAsync(request)).ReturnsAsync(response);
            // Act
            var result = await _controller.CreatePost(request);
            // Assert
            _mockPostService.Verify(s => s.CreatePostAsync(request), Times.Once);
            
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal(201, createdResult.StatusCode);
            Assert.Equal(nameof(PostsController.GetPostById), createdResult.ActionName);
            
            var post = Assert.IsType<PostResponse>(createdResult.Value);
            Assert.Equal(1, post.Id);
            }

        [Fact]
        public async Task Create_Post_InvalidAuthor_ReturnsBadRequest()
            {
            // Arrange

            var request = new FakeBloggingSystem.DTOs.PostRequest
                {
                AuthorId = 122,
                Title = "Test Post",
                Description = "Test Description",
                Content = "Test Content"
                };

            
            _mockPostService.Setup
                (service =>
                service.CreatePostAsync(request)).ReturnsAsync((PostResponse?)null);
            // Act
            var result = await _controller.CreatePost(request);
            // Assert
            _mockPostService.Verify(s => s.CreatePostAsync(request), Times.Once);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal(400, badRequestResult.StatusCode);
            }

        [Fact]
        public async Task Create_GetPostById_ReturnsResultOk()
            {
            // Arrange

            

            var response = new FakeBloggingSystem.DTOs.PostResponse
                {
                Id = 1,
                Title = "Post A"
                };
            _mockPostService.Setup
                (service =>
                service.GetPostByIdAsync(1,false)).ReturnsAsync(response);
            // Act
            var result = await _controller.GetPostById(1,false);

            // Assert
            _mockPostService.Verify(s => s.GetPostByIdAsync(1,false), Times.Once);

            var postResponse = Assert.IsType<OkObjectResult>(result.Result);
            var post = Assert.IsType<PostResponse>(postResponse.Value);


            Assert.Equal(1, post.Id);
            Assert.Null(post.Author);

            }

        [Fact]
        public async Task Create_GetPostById_ReturnsNotFound()
            {
            // Arrange
            
            _mockPostService.Setup
                (service =>
                service.GetPostByIdAsync(122, false)).ReturnsAsync((PostResponse?)null);
            // Act
            var result = await _controller.GetPostById(122, false);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
            }

        [Fact]
        public async Task Create_GetPostById_IncludeAuthorInfo_ReturnsResultOk()
            {
            // Arrange
            var response = new FakeBloggingSystem.DTOs.PostResponse
                {
                Id = 1,
                Title = "Post A",
                Author = new AuthorResponse
                    {
                    Id = 1,
                    Name = "Peter",
                    Surname = "Keli"
                    }
                };
            _mockPostService.Setup
                (service =>
                service.GetPostByIdAsync(1, true)).ReturnsAsync(response);
            // Act
            var result = await _controller.GetPostById(1, true);

            // Assert
            var postResponse = Assert.IsType<OkObjectResult>(result.Result);
            var post = Assert.IsType<PostResponse>(postResponse.Value);
            Assert.NotNull(post.Author);
            Assert.Equal("Peter", post.Author.Name);


            }
        }
        }

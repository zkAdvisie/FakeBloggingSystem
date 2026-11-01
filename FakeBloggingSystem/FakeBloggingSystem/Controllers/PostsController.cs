using FakeBloggingSystem.Data;
using FakeBloggingSystem.DTOs;
using FakeBloggingSystem.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FakeBloggingSystem.Controllers
    {
    [ApiController]
    [Route("post")]
    public class PostsController : ControllerBase
        {
        private readonly IPostService _postService;

        public PostsController(IPostService postService)
            {
            _postService = postService;

            }

        [HttpPost]
        public async Task<ActionResult<PostResponse>> CreatePost(
            [FromBody] PostRequest request)
            {

            var response = await _postService.CreatePostAsync(request);

            if (response == null)
                {
                return BadRequest($"Author with Id {request.AuthorId} does not exist.");
                }

            return CreatedAtAction(nameof(GetPostById), new { id = response.Id }, response);
            }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<PostResponse>> GetPostById(
            int id, [FromQuery] bool includeAuthor = false)
            {
            var response = await _postService
                .GetPostByIdAsync(id, includeAuthor);

            if (response == null)
                {
                return NotFound();
                }

            return Ok(response);
            }
        }
    }

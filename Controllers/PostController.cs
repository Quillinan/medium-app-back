using medium_app_back.Models;
using medium_app_back.Services;
using Microsoft.AspNetCore.Mvc;

namespace medium_app_back.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostController(PostService postService) : ControllerBase
    {
        private readonly PostService postService = postService;

        [HttpGet]
        public async Task<IActionResult> GetAllPosts()
        {
            var posts = await postService.GetAllPostsAsync();
            return Ok(posts);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetPostRequest>> GetPost(int id)
        {
            var post = await postService.GetPostByIdAsync(id);
            if (post == null)
            {
                return NotFound();
            }

            return Ok(post);
        }

        [HttpGet("author/{authorId}")]
        public async Task<IActionResult> GetPostsByAuthorId(string authorId)
        {
            var posts = await postService.GetPostsByAuthorIdAsync(authorId);
            return Ok(posts);
        }

        [HttpPost]
        [RequestSizeLimit(10_000_000)]
        public async Task<IActionResult> AddPost([FromForm] CreatePostRequest createPostRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var post = await postService.AddPostAsync(createPostRequest);
            return CreatedAtAction(nameof(GetPost), new { id = post.Id }, post);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePost(int id, [FromForm] UpdatePostRequest updatedPost)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await postService.UpdatePostAsync(id, updatedPost);
            if (result == null)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(int id)
        {
            var result = await postService.DeletePostAsync(id);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}

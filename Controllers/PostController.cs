using medium_app_back.Models;
using medium_app_back.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace medium_app_back.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostController(PostService postService) : ControllerBase
    {
        [HttpGet]
        [SwaggerOperation(Tags = ["Get Posts"])]
        public async Task<IActionResult> GetAllPosts()
        {
            var posts = await postService.GetAllPostsAsync();
            return Ok(posts);
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Tags = ["Get Posts"])]
        public async Task<IActionResult> GetPostById(int id)
        {
            var post = await postService.GetPostByIdAsync(id);
            if (post == null)
            {
                return NotFound();
            }
            return Ok(post);
        }

        [HttpGet("author/{authorId}")]
        [SwaggerOperation(Tags = ["Get Posts"])]
        public async Task<IActionResult> GetPostsByAuthorId(int authorId)
        {
            var posts = await postService.GetPostsByAuthorIdAsync(authorId);
            return Ok(posts);
        }

        [HttpPost]
        [SwaggerOperation(Tags = ["Create Post"])]

        public async Task<IActionResult> AddPost([FromBody] Post post)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await postService.AddPostAsync(post);
            return CreatedAtAction(nameof(GetPostById), new { id = post.Id }, post);
        }

        [HttpPut("{id}")]
        [SwaggerOperation(Tags = ["Update Post"])]
        public async Task<IActionResult> UpdatePost(int id, [FromBody] Post updatedPost)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await postService.UpdatePostAsync(id, updatedPost);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(Tags = ["Delete Post"])]
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

using medium_app_back.Data;
using medium_app_back.Models;
using medium_app_back.Repositories;

namespace medium_app_back.Services
{
    public class PostService(PostRepository postRepository)
    {
        private static void ValidateTitleAndContent(Post post)
        {
            if (string.IsNullOrWhiteSpace(post.Title) || string.IsNullOrWhiteSpace(post.Content))
            {
                throw new ArgumentException("Title and content are required.");
            }
        }
        
        public async Task<List<Post>> GetAllPostsAsync()
        {
            return await postRepository.GetAllPostsAsync();
        }

        public async Task<Post?> GetPostByIdAsync(int id)
        {
            return await postRepository.GetPostByIdAsync(id);
        }

        public async Task<List<Post>> GetPostsByAuthorIdAsync(int authorId)
        {
            return await postRepository.GetPostsByAuthorIdAsync(authorId);
        }

        public async Task<bool> AddPostAsync(Post post)
        {
            ValidateTitleAndContent(post);

            post.CreatedAt = DateTime.UtcNow;
            post.UpdatedAt = DateTime.UtcNow;
            await postRepository.AddPostAsync(post);
            return true;
        }

        public async Task<bool> UpdatePostAsync(int id, Post updatedPost)
        {
            var existingPost = await GetPostByIdAsync(id);
            if (existingPost == null)
            {
                return false;
            }


            ValidateTitleAndContent(existingPost);


            existingPost.Title = updatedPost.Title;
            existingPost.Content = updatedPost.Content;
            existingPost.UpdatedAt = DateTime.UtcNow;

            await postRepository.UpdatePostAsync(existingPost);
            return true;
        }

        public async Task<bool> DeletePostAsync(int id)
        {
            var existingPost = await GetPostByIdAsync(id);
            if (existingPost == null)
            {
                return false;
            }

            await postRepository.DeletePostAsync(existingPost);
            return true;
        }

    }
}

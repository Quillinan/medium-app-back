using medium_app_back.Data;
using medium_app_back.Models;
using Microsoft.EntityFrameworkCore;

namespace medium_app_back.Repositories
{
    public class PostRepository(AppDbContext context)
    {
        private readonly AppDbContext _context = context;

        public async Task<List<Post>> GetAllPostsAsync()
        {
            return await _context.Posts
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task<Post?> GetPostByIdAsync(int id)
        {
            return await _context.Posts
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<Post>> GetPostsByAuthorIdAsync(int authorId)
        {
            return await _context.Posts
                .Where(p => p.AuthorId == authorId)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task AddUserAsync(Post post)
        {
            post.CreatedAt = DateTime.UtcNow;
            post.UpdatedAt = DateTime.UtcNow;
            _context.Posts.Add(post);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdatePostAsync(int id, Post updatedPost)
        {
            var existingPost = await GetPostByIdAsync(id);
            if (existingPost == null)
            {
                return false;
            }

            existingPost.Title = updatedPost.Title;
            existingPost.Content = updatedPost.Content;
            existingPost.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeletePostAsync(int id)
        {
            var post = await GetPostByIdAsync(id);
            if (post == null)
            {
                return false;
            }

            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
            return true;
        }

    }
}

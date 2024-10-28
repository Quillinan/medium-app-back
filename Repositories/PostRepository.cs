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

        public async Task<List<Post>> GetPostsByAuthorIdAsync(string authorId)
        {
            return await _context.Posts
                .Where(p => p.AuthorId == authorId)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task AddPostAsync(Post post)
        {
            post.CreatedAt = DateTime.UtcNow;
            post.UpdatedAt = DateTime.UtcNow;
            _context.Posts.Add(post);
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePostAsync(Post post)
        {
            _context.Posts.Update(post);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePostAsync(Post post)
        {
            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
        }

    }
}

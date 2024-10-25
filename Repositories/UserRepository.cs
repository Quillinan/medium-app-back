using medium_app_back.Data;
using medium_app_back.Models;

namespace medium_app_back.Repositories
{
    public class UserRepository(AppDbContext context)
    {
        private readonly AppDbContext _context = context;

        public async Task AddUserAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }
    }
}

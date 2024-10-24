using Microsoft.EntityFrameworkCore;
using medium_app_back.Users;

namespace medium_app_back.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
    }
}

using Microsoft.EntityFrameworkCore;
using medium_app_back.Models;

namespace medium_app_back.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Post> Posts { get; set; }
    }
}

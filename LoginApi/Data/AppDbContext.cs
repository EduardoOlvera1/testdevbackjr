using Microsoft.EntityFrameworkCore;
using LoginApi.Models;

namespace LoginApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Login> Logins { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Area> Areas { get; set; }
    }
}
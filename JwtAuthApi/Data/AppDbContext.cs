using JwtAuthApi.Entities.Concrete;
using Microsoft.EntityFrameworkCore;

namespace JwtAuthApi.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {
            
        }
        public DbSet<AppUser> Users { get; set; }
    }

    
}

using COeX_India1._2.Models;
using Microsoft.EntityFrameworkCore;

namespace COeX_India1._2.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Siding> Sidings { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Requests> Requests { get; set; }
    }
}

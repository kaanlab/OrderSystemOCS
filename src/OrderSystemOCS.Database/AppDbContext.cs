using Microsoft.EntityFrameworkCore;
using OrderSystemOCS.Database.Models;
using OrderSystemOCS.Domain;

namespace OrderSystemOCS.Database
{
    public sealed class AppDbContext : DbContext
    {
        public AppDbContext() : base() { }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<OrderDb>()
                .Property(e => e.Status)
                .HasConversion(
                    v => v.ToString(),
                    v => (Status)Enum.Parse(typeof(Status), v));
        }

        public DbSet<OrderDb> Orders { get; set; }
        public DbSet<ProductDb> Products { get; set; }
        public DbSet<LineDb> Lines { get; set; }
    }
}

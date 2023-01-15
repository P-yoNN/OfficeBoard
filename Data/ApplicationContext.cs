using Project.Models;
using Microsoft.EntityFrameworkCore;

namespace Project.Data
{
    public class ApplicationContext : DbContext
    {

        public DbSet<Post> Posts { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Vote> Votes { get; set; }
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder mb)
        {
            mb.Entity<Post>().HasOne(p => p.User).WithMany(u => u.Posts).OnDelete(DeleteBehavior.Cascade);
            mb.Entity<Vote>().HasOne(v => v.Post).WithMany(p => p.Votes);
            mb.Entity<Vote>().HasOne(v => v.User).WithMany(u => u.Votes).OnDelete(DeleteBehavior.Restrict);
        }

    }
}
using BlogTalks.Application.Abstraction;
using BlogTalks.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlogTalks.Infrastructure.Data.DataContext
{
    public sealed class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : DbContext(options), IApplicationDbContext
    {

        public DbSet<BlogPost> BlogPosts { get; set; }

        public DbSet<Comment> Comments { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BlogPost>().HasKey(e => e.Id);
            modelBuilder.Entity<BlogPost>()
                .HasMany(e => e.Comments)
                .WithOne(e => e.BlogPost)
                .HasForeignKey(e => e.BlogPostId)
                .IsRequired();

            modelBuilder.Entity<Comment>().HasKey(e => e.Id);

            base.OnModelCreating(modelBuilder);
        }
    }
}

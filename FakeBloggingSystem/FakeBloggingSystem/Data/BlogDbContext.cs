using Microsoft.EntityFrameworkCore;
using FakeBloggingSystem.Models;

namespace FakeBloggingSystem.Data
    {
    public class BlogDbContext : DbContext
        {
        public BlogDbContext(DbContextOptions<BlogDbContext> options) : base(options)
            {
            }

        public DbSet<Author> Authors => Set<Author>();
        public DbSet<Post> Posts => Set<Post>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Post>()
                .HasOne(p => p.Author)
                .WithMany(a => a.Posts)
                .HasForeignKey(p=>p.AuthorId)
                .IsRequired();

            modelBuilder.Entity<Post>()
                .Property(p => p.Title)
                .HasMaxLength(200)
                .IsRequired();

            modelBuilder.Entity<Author>()
                .Property(a => a.Name)
                .HasMaxLength(100)
                .IsRequired();

            modelBuilder.Entity<Author>()
                .Property(a => a.Surname)
                .HasMaxLength(100)
                .IsRequired();

            modelBuilder.Entity<Author>()
                .HasMany(a => a.Posts)
                .WithOne(p => p.Author)
                .IsRequired();
            }
        }
    }

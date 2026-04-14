using Microsoft.EntityFrameworkCore;
using UttuHub.API.Models;

namespace UttuHub.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){ } // Constructor for the DbContext re hai


        // Tables xai aba haalni re :
        public DbSet<Category> Categories { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<FeedItem> FeedItems { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<User> Users { get; set; }


        // NEW: Junction table for many-to-many (Copied from Gemini cause bro wut 0_0)
        public DbSet<FeedItemCategory> FeedItemCategories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 1. Configure Composite Key for the Many-to-Many Junction Table
            modelBuilder.Entity<FeedItemCategory>()
                .HasKey(fc => new { fc.FeedItemId, fc.CategoryId });

            // 2. Configure the relationship: FeedItem -> FeedItemCategory
            modelBuilder.Entity<FeedItemCategory>()
                .HasOne(fc => fc.FeedItem)
                .WithMany(f => f.FeedItemCategories)
                .HasForeignKey(fc => fc.FeedItemId);

            // 3. Configure the relationship: Category -> FeedItemCategory
            modelBuilder.Entity<FeedItemCategory>()
                .HasOne(fc => fc.Category)
                .WithMany(c => c.FeedItemCategories)
                .HasForeignKey(fc => fc.CategoryId);

            // 4. Existing User Relationships (One-to-Many)
            modelBuilder.Entity<FeedItem>()
                .HasOne(f => f.User)
                .WithMany(u => u.FeedItems)
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }

    }
}


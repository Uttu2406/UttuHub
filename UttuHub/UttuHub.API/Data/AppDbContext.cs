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


        // OnModelCreating (Optional re so pasted from Gemini)
        // This is where you can manually tell EF how to handle the relationship if it gets confused.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // This ensures that if you delete a Category, it doesn't accidentally crash the database if it has FeedItems.
            modelBuilder.Entity<FeedItem>()
                .HasOne(f => f.Category).WithMany(c => c.FeedItems)
                .HasForeignKey(f => f.CategoryId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}


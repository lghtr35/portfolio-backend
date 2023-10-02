using Microsoft.EntityFrameworkCore;
using Portfolio.Backend.Common.Data.Entities;

namespace Portfolio.Backend.Common.Data.Repository
{
    public class AppDatabaseContext : DbContext
    {
        public AppDatabaseContext(DbContextOptions<AppDatabaseContext> options)
            : base(options)
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }
        public DbSet<Image> Images { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Admin> Admins { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Admin Infrastructure
            modelBuilder.Entity<Admin>().HasKey(prop => prop.UserId);

            // Project Relations and Infrastructure
            modelBuilder.Entity<Project>()
                .HasMany(e => e.Images)
                .WithOne(e => e.Project)
                .HasForeignKey(e => e.ProjectId)
                .HasPrincipalKey(e => e.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Project>()
                .Property(e => e.CreatedAt)
                .HasDefaultValueSql("getutcdate()");

            modelBuilder.Entity<Project>()
                .Property(e => e.UpdatedAt)
                .HasDefaultValueSql("getutcdate()");

            // Image Relations and Infraclassure
            modelBuilder.Entity<Image>()
                .Property(e => e.CreatedAt)
                .HasDefaultValueSql("getutcdate()");

            modelBuilder.Entity<Image>()
                .Property(e => e.UpdatedAt)
                .HasDefaultValueSql("getutcdate()");

            // Base ORM
            base.OnModelCreating(modelBuilder);
        }
    }
}
using Microsoft.EntityFrameworkCore;
using Portfolio.Backend.Common.Data.Entities;

namespace Portfolio.Backend.Common.Data.Repository
{
    public class AppDatabaseContext : DbContext
    {
        public AppDatabaseContext(DbContextOptions<AppDatabaseContext> options)
            : base(options)
        {
            Database.Migrate();
        }
        public DbSet<Image> Images { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Content> Contents { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Admin Relations and Infrastructure
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
                .HasDefaultValueSql("NOW()");

            modelBuilder.Entity<Project>()
                .Property(e => e.UpdatedAt)
                .HasDefaultValueSql("NOW()");

            // Image Relations and Infraclassure
            modelBuilder.Entity<Image>()
                .Property(e => e.CreatedAt)
                .HasDefaultValueSql("NOW()");

            modelBuilder.Entity<Image>()
                .Property(e => e.UpdatedAt)
                .HasDefaultValueSql("NOW()");

            // Content Relations and Infrastructure
            modelBuilder.Entity<Content>().HasKey(e => e.ContentId);

            modelBuilder.Entity<Content>()
                .Property(e => e.CreatedAt)
                .HasDefaultValueSql("NOW()");

            modelBuilder.Entity<Content>()
                .Property(e => e.UpdatedAt)
                .HasDefaultValueSql("NOW()");
            // Base ORM
            base.OnModelCreating(modelBuilder);
        }

    }
}
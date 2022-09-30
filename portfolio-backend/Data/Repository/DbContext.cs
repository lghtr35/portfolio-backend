using Microsoft.EntityFrameworkCore;
using portfolio_backend.Models.Entities;

namespace portfolio_backend.Models.Repository
{
    public class AppDatabaseContext : DbContext
    {
        public AppDatabaseContext(DbContextOptions<AppDatabaseContext> options)
            : base(options)
        {
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
            modelBuilder.Entity<Project>().HasKey(prop => prop.ProjectId);
            modelBuilder.Entity<Project>().Property(prop => prop.CreatedAt).HasDefaultValueSql("getutcdate()");
            modelBuilder.Entity<Project>().Property(prop => prop.UpdatedAt).HasDefaultValueSql("getutcdate()");
            modelBuilder.Entity<Project>().HasMany(prop => prop.Images).WithOne();
            // Image Relations and Infrastructure
            modelBuilder.Entity<Image>().HasKey(prop => prop.ImageId);
            modelBuilder.Entity<Image>().Property(prop => prop.CreatedAt).HasDefaultValueSql("getutcdate()");
            modelBuilder.Entity<Image>().Property(prop => prop.UpdatedAt).HasDefaultValueSql("getutcdate()");
            modelBuilder.Entity<Image>().HasOne(prop => prop.Project).WithMany();
            // Base ORM
            base.OnModelCreating(modelBuilder);
        }
    }
}
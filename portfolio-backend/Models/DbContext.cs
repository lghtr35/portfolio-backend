using Microsoft.EntityFrameworkCore;

namespace backend.Models
{
    public class AppDatabaseContext : DbContext
    {
        public AppDatabaseContext(DbContextOptions<AppDatabaseContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Admin> Admins { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Admin Infrastructure
            modelBuilder.Entity<Admin>().HasKey(prop => prop.UserId);
            // Post Relations and Infrastructure
            modelBuilder.Entity<Post>().HasKey(prop => prop.PostId);
            modelBuilder.Entity<Post>().Property(prop => prop.CreatedAt).HasDefaultValueSql("getutcdate()");
            modelBuilder.Entity<Post>().Property(prop => prop.UpdatedAt).HasDefaultValueSql("getutcdate()");
            modelBuilder.Entity<Post>().HasMany(prop => prop.Images).WithOne().HasForeignKey(prop => prop.ImageId);
            // Project Relations and Infrastructure
            modelBuilder.Entity<Project>().HasKey(prop => prop.ProjectId);
            modelBuilder.Entity<Project>().Property(prop => prop.CreatedAt).HasDefaultValueSql("getutcdate()");
            modelBuilder.Entity<Project>().Property(prop => prop.UpdatedAt).HasDefaultValueSql("getutcdate()");
            modelBuilder.Entity<Project>().HasMany(prop => prop.Images).WithOne().HasForeignKey(prop => prop.ImageId);
            // Image Relations and Infrastructure
            modelBuilder.Entity<Image>().HasKey(prop => prop.ImageId);
            modelBuilder.Entity<Image>().Property(prop => prop.CreatedAt).HasDefaultValueSql("getutcdate()");
            modelBuilder.Entity<Image>().Property(prop => prop.UpdatedAt).HasDefaultValueSql("getutcdate()");
            // Base ORM
            base.OnModelCreating(modelBuilder);
        }
    }
}
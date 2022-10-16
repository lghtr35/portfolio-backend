using Microsoft.EntityFrameworkCore;
using portfolio_backend.Data.Entities;

namespace portfolio_backend.Data.Repository
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
            modelBuilder.Entity<Project>(entity =>
            {
                entity.HasKey(prop => prop.ProjectId);
                entity.Property(prop => prop.ProjectId).ValueGeneratedOnAdd();
                entity.Property(prop => prop.CreatedAt).HasDefaultValueSql("getutcdate()");
                entity.Property(prop => prop.UpdatedAt).HasDefaultValueSql("getutcdate()");
            });
            // Image Relations and Infrastructure
            modelBuilder.Entity<Image>(entity =>
            {
                entity.HasKey(prop => prop.ImageId);
                entity.Property(prop => prop.CreatedAt).HasDefaultValueSql("getutcdate()");
                entity.Property(prop => prop.UpdatedAt).HasDefaultValueSql("getutcdate()");
                entity.HasOne(prop => prop.Project).WithMany(prop_else => prop_else.Images).HasForeignKey(prop => prop.ProjectId).OnDelete(DeleteBehavior.SetNull);
            });

            // Base ORM
            base.OnModelCreating(modelBuilder);
        }
    }
}
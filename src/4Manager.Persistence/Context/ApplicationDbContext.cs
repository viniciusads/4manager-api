using _4Tech._4Manager.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace _4Manager.Persistence.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Customer> Customers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");

                entity.HasKey(u => u.UserId);

                entity.Property(u => u.Name).IsRequired().HasMaxLength(100);
                entity.Property(u => u.Email).IsRequired().HasMaxLength(100);
                entity.Property(u => u.IsActive).IsRequired();
                entity.Property(u => u.Role).IsRequired();
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("customers");

                entity.HasKey(c => c.CustomerId);

                entity.Property(c => c.CustomerId)
                    .HasDefaultValueSql("gen_random_uuid()");

                entity.Property(c => c.Name).IsRequired().HasMaxLength(100);
                entity.Property(c => c.IsActive).IsRequired();
                entity.Property(c => c.CreatedAt).IsRequired();
            });
        }
    }
}

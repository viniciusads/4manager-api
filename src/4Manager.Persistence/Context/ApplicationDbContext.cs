using _4Manager.Domain.Entities;
using Microsoft.EntityFrameworkCore;


namespace _4Manager.Persistence.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.ToTable("Usuarios");

                entity.HasKey(u => u.Id);

                entity.Property(u => u.Nome).IsRequired().HasMaxLength(100);
                entity.Property(u => u.Email).IsRequired().HasMaxLength(100);
                entity.Property(u => u.SenhaHash).IsRequired().HasMaxLength(255);
                entity.Property(u => u.Ativo).IsRequired();
            });
        }
    }
}

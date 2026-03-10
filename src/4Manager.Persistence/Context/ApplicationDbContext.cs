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
        public DbSet<Project> Projects { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<TeamCollaborator> TeamCollaborator { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<TicketDetails> TicketDetails { get; set; }
        public DbSet<MessageHistory> MessageHistories { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Timesheet> Timesheets { get; set; }

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

            modelBuilder.Entity<Project>(entity =>
            {
                entity.ToTable("projects");

                entity.HasKey(e => e.ProjectId);

                entity.Property(e => e.ProjectId)
                    .HasDefaultValueSql("gen_random_uuid()");

                entity.HasOne(e => e.Customer)
                    .WithMany(c => c.Projects)
                    .HasForeignKey(e => e.CustomerId)
                    .OnDelete(DeleteBehavior.SetNull);
                
                entity.HasOne(e => e.CustomerManager)
                    .WithMany()
                    .HasForeignKey(e => e.CustomerManagerId)
                    .OnDelete(DeleteBehavior.SetNull);
                
                entity.HasOne(e => e.Team)
                      .WithOne(t => t.Project)
                      .HasForeignKey<Team>(t => t.ProjectId)
                      .IsRequired()
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(e => e.Tickets)
                      .WithOne(t => t.Project)
                      .HasForeignKey(t => t.ProjectId)
                      .IsRequired()
                      .OnDelete(DeleteBehavior.Cascade);

                entity.Property(e => e.ProjectName).IsRequired().HasMaxLength(255);
                entity.Property(e => e.StatusProject).IsRequired();
                entity.Property(e => e.StartDate).IsRequired();
                entity.Property(e => e.DeliveryDate).IsRequired();
                entity.Property(e => e.TitleColor).HasMaxLength(50);

                entity.Property(e => e.StatusTime).HasMaxLength(50);
                entity.Property(e => e.Favorite).HasDefaultValue(false);
                entity.Property(e => e.Archived).HasDefaultValue(false);
            });

            modelBuilder.Entity<Team>(entity =>
            {
                entity.ToTable("teams");
                entity.HasKey(e => e.TeamId);
                entity.Property(e => e.TeamId)
                    .HasDefaultValueSql("gen_random_uuid()");

                
                entity.HasIndex(e => e.ProjectId).IsUnique();

                entity.HasOne(e => e.Manager)
                    .WithMany()
                    .HasForeignKey(e => e.ManagerId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Restrict);
            });


            modelBuilder.Entity<TeamCollaborator>(entity =>
            {
                entity.ToTable("team_collaborators");

                entity.HasKey(e => e.TeamCollaboratorId);

                entity.Property(e => e.TeamCollaboratorId)
                    .HasDefaultValueSql("gen_random_uuid()");

                entity.HasOne(e => e.Team)
                      .WithMany(t => t.Collaborators)
                      .HasForeignKey(e => e.TeamId)
                      .IsRequired()
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.User)
                      .WithMany(u => u.TeamCollaborators)
                      .HasForeignKey(e => e.CollaboratorId)
                      .IsRequired()
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Ticket>(entity =>
            {
                entity.ToTable("tickets");
                entity.HasKey(e => e.TicketId);

                entity.HasOne(t => t.Project)
                      .WithMany(p => p.Tickets)
                      .HasForeignKey(t => t.ProjectId)
                      .IsRequired()
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(t => t.Attachments)
                      .WithOne(a => a.Ticket)
                      .HasForeignKey(a => a.TicketId)
                      .IsRequired()
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<TicketAttachment>(entity =>
            {
                entity.ToTable("ticketAttachment");
                entity.HasKey(a => a.AttachmentId);
                entity.Property(a => a.FileName).IsRequired();
                entity.Property(a => a.FilePath).IsRequired();

                entity.HasOne(a => a.Ticket)
                      .WithMany(t => t.Attachments)
                      .HasForeignKey(a => a.TicketId)
                      .IsRequired()
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<TicketDetails>(entity =>
            {
                entity.ToTable("ticketDetails");
                entity.HasKey(e => e.TicketDetailsId);

                entity.HasOne(td => td.Ticket)
                      .WithOne(t => t.TicketDetails)
                      .HasForeignKey<TicketDetails>(td => td.TicketId)
                      .IsRequired()
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Note>(entity =>
            {
                entity.ToTable("note");
                entity.HasKey(e => e.NoteId);

                entity.HasOne<TicketDetails>()
                      .WithMany(td => td.Note)
                      .HasForeignKey(e => e.TicketDetailsId)
                      .IsRequired()
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<MessageHistory>(entity =>
            {
                entity.ToTable("messageHistory");
                entity.HasKey(e => e.MessageHistoryId);

                entity.HasOne<TicketDetails>()
                      .WithMany(td => td.MessageHistory)
                      .HasForeignKey(e => e.TicketDetailsId)
                      .IsRequired()
                      .OnDelete(DeleteBehavior.Cascade);
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

            modelBuilder.Entity<Timesheet>(entity =>
            {
                entity.ToTable("timesheets");

                entity.HasKey(t => t.TimesheetId);

                entity.Property(t => t.TimesheetId)
                        .HasDefaultValueSql("gen_random_uuid()");

                entity.Property(t => t.Date).HasColumnType("date").IsRequired();
                entity.Property(t => t.StartDate).IsRequired();
                entity.Property(t => t.EndDate).IsRequired(false);

                entity.Property(t => t.Description);
                entity.Property(t => t.BlockColor);

                entity.HasOne<User>()
                      .WithMany()
                      .HasForeignKey(t => t.UserId)
                      .IsRequired(false)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne<Project>()
                      .WithMany()
                      .HasForeignKey(t => t.ProjectId)
                      .IsRequired(false)
                      .OnDelete(DeleteBehavior.Cascade);

            });
        }
    }
}
namespace FP_FAP.Models;

using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

public class ProjectDbContext : DbContext
{
    public ProjectDbContext(IConfiguration configuration)
    {
        this.Configuration = configuration;
    }

    private IConfiguration Configuration { get; }

    public DbSet<User>                 Users                 { get; set; } = null!;
    public DbSet<Group>                Groups                { get; set; } = null!;
    public DbSet<Subject>              Subjects              { get; set; } = null!;
    public DbSet<Feedback>             Feedbacks             { get; set; } = null!;
    public DbSet<SupportTicket>        SupportTickets        { get; set; } = null!;
    public DbSet<SupportTicketMessage> SupportTicketMessages { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseMySql(
                this.Configuration.GetConnectionString("Db"),
                ServerVersion.Create(new Version(8, 0, 21), ServerType.MySql)
            );
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
                    .HasIndex(u => u.Email)
                    .IsUnique();

        modelBuilder.Entity<Group>()
                    .HasIndex(g => new { g.SubjectId, g.Name, g.Semester })
                    .IsUnique();

        modelBuilder.Entity<Subject>()
                    .HasIndex(s => s.Code)
                    .IsUnique();

        modelBuilder.Entity<Feedback>()
                    .HasIndex(f => new { f.GroupId, f.StudentId })
                    .IsUnique();

        modelBuilder.Entity<Enroll>()
                    .HasKey(e => new { e.GroupId, e.StudentId });

        modelBuilder.Entity<SupportTicket>();

        modelBuilder.Entity<SupportTicketMessage>();
    }
}

public static class EnsureMigration
{
    public static IApplicationBuilder UseEnsureMigration(this IApplicationBuilder builder)
    {
        using var scope     = builder.ApplicationServices.CreateScope();
        var       dbContext = scope.ServiceProvider.GetService<ProjectDbContext>();
        dbContext!.Database.Migrate();

        return builder;
    }
}
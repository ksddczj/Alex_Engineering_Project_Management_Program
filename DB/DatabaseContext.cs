using Microsoft.EntityFrameworkCore;

public class DatabaseContext : DbContext
{
    public DbSet<Project> Projects { get; set; }

    public DbSet<Stage> Stages { get; set; } //this is not necessary we never need to access context.stages

    //always access stage through project

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite(
            @"Data Source=C:\Users\ksddc\Documents\Codes\Database\Project_Management_Program\ProgramDB.db"
        );
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Project>(entity =>
        {
            entity.HasKey(p => p.ProjectID);

            entity.Property(p => p.ProjectName).IsRequired().HasMaxLength(100);

            entity.Property(p => p.Client).IsRequired().HasMaxLength(100);

            entity.Property(p => p.CompletionDate).IsRequired();

            entity.Property(p => p.ProjectID).IsRequired().HasMaxLength(10);

            entity.Property(p => p.Type).HasConversion<string>().IsRequired();

            entity
                .HasMany(p => p.Stages)
                .WithOne()
                .HasForeignKey(s => s.ProjectID)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Stage>(entity =>
        {
            entity.HasKey(s => new { s.StageName, s.ProjectID });

            entity.Property(s => s.StageName).HasConversion<string>().IsRequired();

            entity.Property(s => s.Deadline).IsRequired();

            entity.Property(s => s.IsCompleted).IsRequired();

            entity.Property(s => s.ProjectID).IsRequired().HasMaxLength(10);
        });

        // Configuration for AutomotiveEngineeringProject
        modelBuilder.Entity<AutomotiveEngineeringProject>(entity =>
        {
            entity.HasBaseType<Project>(); // Inherit from Project
        });

        // Configuration for EngineeringDraftingProject
        modelBuilder.Entity<EngineeringDraftingProject>(entity =>
        {
            entity.HasBaseType<Project>(); // Inherit from Project
        });
    }
}

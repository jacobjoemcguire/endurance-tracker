using EnduranceTracker.Core.Entities;
using EnduranceTracker.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EnduranceTracker.Infrastructure.Data;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Activity> Activities => Set<Activity>();
    public DbSet<StrengthDetail> StrengthDetails => Set<StrengthDetail>();
    public DbSet<Event> Events => Set<Event>();
    public DbSet<TrainingPlan> TrainingPlans => Set<TrainingPlan>();
    public DbSet<PlannedSession> PlannedSessions => Set<PlannedSession>();
    public DbSet<SavedView> SavedViews => Set<SavedView>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}

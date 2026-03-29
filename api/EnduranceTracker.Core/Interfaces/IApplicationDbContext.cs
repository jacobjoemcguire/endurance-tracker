using EnduranceTracker.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace EnduranceTracker.Core.Interfaces;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; }
    DbSet<Activity> Activities { get; }
    DbSet<StrengthDetail> StrengthDetails { get; }
    DbSet<Event> Events { get; }
    DbSet<TrainingPlan> TrainingPlans { get; }
    DbSet<PlannedSession> PlannedSessions { get; }
    DbSet<SavedView> SavedViews { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

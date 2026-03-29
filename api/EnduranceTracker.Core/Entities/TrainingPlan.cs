using EnduranceTracker.Core.Enums;

namespace EnduranceTracker.Core.Entities;

public class TrainingPlan
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid EventId { get; set; }
    public DateTime GeneratedDate { get; set; }
    public PlanStatus Status { get; set; }
    public string? PlanJson { get; set; }

    public User User { get; set; } = null!;
    public Event Event { get; set; } = null!;
    public ICollection<PlannedSession> Sessions { get; set; } = new List<PlannedSession>();
}

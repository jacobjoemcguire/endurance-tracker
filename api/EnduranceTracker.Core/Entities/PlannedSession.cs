using EnduranceTracker.Core.Enums;

namespace EnduranceTracker.Core.Entities;

public class PlannedSession
{
    public Guid Id { get; set; }
    public Guid PlanId { get; set; }
    public DateTime Date { get; set; }
    public ActivityType Type { get; set; }
    public string Description { get; set; } = string.Empty;
    public TimeSpan? TargetDuration { get; set; }
    public double? TargetDistance { get; set; }
    public string? TargetIntensity { get; set; }
    public Guid? CompletedActivityId { get; set; }

    public TrainingPlan Plan { get; set; } = null!;
    public Activity? CompletedActivity { get; set; }
}

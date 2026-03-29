using EnduranceTracker.Core.Enums;

namespace EnduranceTracker.Core.Entities;

public class Event
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public Discipline Discipline { get; set; }
    public double? Distance { get; set; }
    public double? ElevationGain { get; set; }
    public TimeSpan? TargetTime { get; set; }
    public int Priority { get; set; }
    public string? Notes { get; set; }

    public User User { get; set; } = null!;
    public TrainingPlan? TrainingPlan { get; set; }
}

using EnduranceTracker.Core.Enums;

namespace EnduranceTracker.Core.Entities;

public class Activity
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public ActivitySource Source { get; set; }
    public long? StravaActivityId { get; set; }
    public ActivityType Type { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public TimeSpan? Duration { get; set; }
    public double? Distance { get; set; }
    public double? ElevationGain { get; set; }
    public double? AvgSpeed { get; set; }
    public double? AvgPace { get; set; }
    public int? AvgHeartRate { get; set; }
    public int? MaxHeartRate { get; set; }
    public int? Calories { get; set; }
    public string? RawJson { get; set; }

    public User User { get; set; } = null!;
    public ICollection<StrengthDetail> StrengthDetails { get; set; } = new List<StrengthDetail>();
}

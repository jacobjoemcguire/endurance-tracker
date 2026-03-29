namespace EnduranceTracker.Core.Entities;

public class StrengthDetail
{
    public Guid Id { get; set; }
    public Guid ActivityId { get; set; }
    public string Exercise { get; set; } = string.Empty;
    public int? Sets { get; set; }
    public int? Reps { get; set; }
    public double? Weight { get; set; }
    public string? Notes { get; set; }

    public Activity Activity { get; set; } = null!;
}

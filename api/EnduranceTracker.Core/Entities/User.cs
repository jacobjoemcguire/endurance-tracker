namespace EnduranceTracker.Core.Entities;

public class User
{
    public Guid Id { get; set; }
    public string EntraObjectId { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public long? StravaAthleteId { get; set; }
    public string? EncryptedStravaAccessToken { get; set; }
    public string? EncryptedStravaRefreshToken { get; set; }
    public DateTime? StravaTokenExpiry { get; set; }

    public ICollection<Activity> Activities { get; set; } = new List<Activity>();
    public ICollection<Event> Events { get; set; } = new List<Event>();
    public ICollection<TrainingPlan> TrainingPlans { get; set; } = new List<TrainingPlan>();
    public ICollection<SavedView> SavedViews { get; set; } = new List<SavedView>();
}

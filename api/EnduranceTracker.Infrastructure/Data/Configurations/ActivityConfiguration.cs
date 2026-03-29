using EnduranceTracker.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EnduranceTracker.Infrastructure.Data.Configurations;

public class ActivityConfiguration : IEntityTypeConfiguration<Activity>
{
    public void Configure(EntityTypeBuilder<Activity> builder)
    {
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Name).IsRequired().HasMaxLength(500);
        builder.Property(a => a.RawJson).HasColumnType("nvarchar(max)");

        builder.HasIndex(a => a.UserId);
        builder.HasIndex(a => new { a.UserId, a.Date });
        builder.HasIndex(a => a.StravaActivityId).IsUnique()
            .HasFilter("[StravaActivityId] IS NOT NULL");

        builder.HasOne(a => a.User)
            .WithMany(u => u.Activities)
            .HasForeignKey(a => a.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

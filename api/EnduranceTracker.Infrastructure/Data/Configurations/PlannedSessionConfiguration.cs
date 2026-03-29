using EnduranceTracker.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EnduranceTracker.Infrastructure.Data.Configurations;

public class PlannedSessionConfiguration : IEntityTypeConfiguration<PlannedSession>
{
    public void Configure(EntityTypeBuilder<PlannedSession> builder)
    {
        builder.HasKey(ps => ps.Id);
        builder.Property(ps => ps.Description).IsRequired().HasMaxLength(1000);
        builder.Property(ps => ps.TargetIntensity).HasMaxLength(200);

        builder.HasOne(ps => ps.Plan)
            .WithMany(tp => tp.Sessions)
            .HasForeignKey(ps => ps.PlanId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(ps => ps.CompletedActivity)
            .WithMany()
            .HasForeignKey(ps => ps.CompletedActivityId)
            .OnDelete(DeleteBehavior.SetNull)
            .IsRequired(false);
    }
}

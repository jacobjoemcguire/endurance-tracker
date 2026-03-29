using EnduranceTracker.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EnduranceTracker.Infrastructure.Data.Configurations;

public class TrainingPlanConfiguration : IEntityTypeConfiguration<TrainingPlan>
{
    public void Configure(EntityTypeBuilder<TrainingPlan> builder)
    {
        builder.HasKey(tp => tp.Id);
        builder.Property(tp => tp.PlanJson).HasColumnType("nvarchar(max)");

        builder.HasOne(tp => tp.User)
            .WithMany(u => u.TrainingPlans)
            .HasForeignKey(tp => tp.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(tp => tp.Event)
            .WithOne(e => e.TrainingPlan)
            .HasForeignKey<TrainingPlan>(tp => tp.EventId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}

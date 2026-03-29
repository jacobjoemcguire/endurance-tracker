using EnduranceTracker.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EnduranceTracker.Infrastructure.Data.Configurations;

public class StrengthDetailConfiguration : IEntityTypeConfiguration<StrengthDetail>
{
    public void Configure(EntityTypeBuilder<StrengthDetail> builder)
    {
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Exercise).IsRequired().HasMaxLength(200);

        builder.HasOne(s => s.Activity)
            .WithMany(a => a.StrengthDetails)
            .HasForeignKey(s => s.ActivityId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

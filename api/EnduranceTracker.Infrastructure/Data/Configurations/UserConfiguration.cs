using EnduranceTracker.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EnduranceTracker.Infrastructure.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);
        builder.Property(u => u.EntraObjectId).IsRequired().HasMaxLength(36);
        builder.Property(u => u.DisplayName).IsRequired().HasMaxLength(200);

        builder.HasIndex(u => u.EntraObjectId).IsUnique();
        builder.HasIndex(u => u.StravaAthleteId).IsUnique()
            .HasFilter("[StravaAthleteId] IS NOT NULL");
    }
}

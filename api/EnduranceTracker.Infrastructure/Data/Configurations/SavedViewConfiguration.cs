using EnduranceTracker.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EnduranceTracker.Infrastructure.Data.Configurations;

public class SavedViewConfiguration : IEntityTypeConfiguration<SavedView>
{
    public void Configure(EntityTypeBuilder<SavedView> builder)
    {
        builder.HasKey(sv => sv.Id);
        builder.Property(sv => sv.Name).IsRequired().HasMaxLength(200);
        builder.Property(sv => sv.FilterJson).IsRequired().HasColumnType("nvarchar(max)");

        builder.HasIndex(sv => sv.UserId);

        builder.HasOne(sv => sv.User)
            .WithMany(u => u.SavedViews)
            .HasForeignKey(sv => sv.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

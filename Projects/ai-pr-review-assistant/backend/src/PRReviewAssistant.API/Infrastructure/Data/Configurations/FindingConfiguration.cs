using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PRReviewAssistant.API.Domain.Entities;
using PRReviewAssistant.API.Domain.Enums;

namespace PRReviewAssistant.API.Infrastructure.Data.Configurations;

public class FindingConfiguration : IEntityTypeConfiguration<Finding>
{
    public void Configure(EntityTypeBuilder<Finding> builder)
    {
        builder.ToTable("Findings");

        builder.HasKey(f => f.Id);

        builder.Property(f => f.Id)
            .HasMaxLength(26)
            .IsRequired();

        builder.Property(f => f.ReviewId)
            .HasMaxLength(26)
            .IsRequired();

        builder.Property(f => f.Category)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(f => f.Severity)
            .HasConversion<string>()
            .HasMaxLength(10)
            .IsRequired();

        builder.Property(f => f.Title)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(f => f.Description)
            .HasMaxLength(2000)
            .IsRequired();

        builder.Property(f => f.LineReference)
            .HasMaxLength(50);

        builder.Property(f => f.Suggestion)
            .HasMaxLength(2000)
            .IsRequired();

        builder.Property(f => f.Confidence)
            .IsRequired();

        builder.Property(f => f.SuggestedFix)
            .HasMaxLength(5000);

        builder.HasIndex(f => f.ReviewId);
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PRReviewAssistant.API.Domain.Entities;

namespace PRReviewAssistant.API.Infrastructure.Data.Configurations;

public class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.ToTable("Reviews");

        builder.HasKey(r => r.Id);

        builder.Property(r => r.Id)
            .HasMaxLength(26)
            .IsRequired();

        builder.Property(r => r.CodeDiff)
            .HasMaxLength(50_000)
            .IsRequired();

        builder.Property(r => r.Language)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(r => r.PrUrl)
            .HasMaxLength(2048);

        builder.Property(r => r.CreatedAt)
            .IsRequired();

        builder.HasMany(r => r.Findings)
            .WithOne(f => f.Review)
            .HasForeignKey(f => f.ReviewId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(r => r.CreatedAt)
            .IsDescending();
    }
}

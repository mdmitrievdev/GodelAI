using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PRReviewAssistant.API.Domain.Entities;

namespace PRReviewAssistant.API.Infrastructure.Data.Configurations;

public class AppSettingsConfiguration : IEntityTypeConfiguration<AppSettings>
{
    public void Configure(EntityTypeBuilder<AppSettings> builder)
    {
        builder.ToTable("AppSettings");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Id)
            .HasMaxLength(26)
            .IsRequired();

        builder.Property(s => s.UseMockAi)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(s => s.AiModel)
            .HasMaxLength(100)
            .IsRequired()
            .HasDefaultValue("mock");

        builder.HasData(new AppSettings
        {
            Id = "default",
            UseMockAi = true,
            AiModel = "mock"
        });
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WareHouseManagement.Domain.Entities;

namespace WareHouseManagement.Infrastructure.Data.Configurations;

public class UnitTypeRuleConfiguration : IEntityTypeConfiguration<UnitTypeRule>
{
    public void Configure(EntityTypeBuilder<UnitTypeRule> builder)
    {
        builder.HasKey(u => u.Id);

        builder.Property(u => u.NameKa)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(u => u.NameEn)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(u => u.Abbreviation)
            .IsRequired()
            .HasMaxLength(10);

        builder.Property(u => u.Description)
            .HasMaxLength(500);

        builder.Property(u => u.MinValue)
            .HasPrecision(18, 2);

        builder.Property(u => u.MaxValue)
            .HasPrecision(18, 2);

        builder.Property(u => u.DefaultValue)
            .HasPrecision(18, 2);

        // UnitType უნიკალური უნდა იყოს
        builder.HasIndex(u => u.UnitType)
            .IsUnique();

        builder.HasQueryFilter(u => !u.IsDeleted);
    }
}


using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WareHouseManagement.Domain.Entities;

namespace WareHouseManagement.Infrastructure.Data.Configurations;

public class DebtorConfiguration : IEntityTypeConfiguration<Debtor>
{
    public void Configure(EntityTypeBuilder<Debtor> builder)
    {
        builder.HasKey(d => d.Id);

        builder.Property(d => d.DebtorName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(d => d.Phone)
            .HasMaxLength(50);

        builder.Property(d => d.Email)
            .HasMaxLength(100);

        builder.Property(d => d.TotalDebt)
            .HasPrecision(18, 2);

        builder.Property(d => d.PaidAmount)
            .HasPrecision(18, 2);

        builder.Property(d => d.RemainingDebt)
            .HasPrecision(18, 2);

        builder.HasOne(d => d.Company)
            .WithMany()
            .HasForeignKey(d => d.CompanyId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasQueryFilter(d => !d.IsDeleted);
    }
}


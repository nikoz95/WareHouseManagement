using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WareHouseManagement.Domain.Entities;

namespace WareHouseManagement.Infrastructure.Data.Configurations;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable("RefreshTokens");
        
        builder.HasKey(rt => rt.Id);
        
        builder.Property(rt => rt.Token)
            .IsRequired()
            .HasMaxLength(500);
        
        builder.Property(rt => rt.RevokedReason)
            .HasMaxLength(200);
        
        builder.Property(rt => rt.ReplacedByToken)
            .HasMaxLength(500);
        
        builder.Property(rt => rt.IpAddress)
            .HasMaxLength(50);
        
        // Indexes
        builder.HasIndex(rt => rt.Token)
            .IsUnique();
        
        builder.HasIndex(rt => rt.UserId);
        
        builder.HasIndex(rt => rt.ExpiresAt);
        
        // Ignore calculated properties
        builder.Ignore(rt => rt.IsExpired);
        builder.Ignore(rt => rt.IsActive);
        
        // Relationships
        builder.HasOne(rt => rt.User)
            .WithMany(u => u.RefreshTokens)
            .HasForeignKey(rt => rt.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}


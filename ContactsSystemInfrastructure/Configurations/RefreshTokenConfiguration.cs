
using ContactsSystemDomain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ContactsSystemInfrastructure.Configurations;

public sealed class RefreshTokenConfiguration
    : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> entity)
    {
        entity.ToTable("RefreshTokens");

        entity.HasKey(e => e.Id);

        entity.Property(e => e.Id)
            .HasColumnName("ID");

        entity.Property(e => e.UserId)
            .HasColumnName("UserID");

        entity.Property(e => e.TokenHash)
            .HasMaxLength(64)
            .IsUnicode()
            .IsRequired();

        entity.Property(e => e.ExpiresAt)
            .HasColumnType("datetime2");

        entity.Property(e => e.CreatedAt)
            .HasColumnType("datetime2")
            .HasDefaultValueSql("SYSUTCDATETIME()");

        entity.Property(e => e.RevokedAt)
            .HasColumnType("datetime2");

        entity.HasIndex(e => e.TokenHash)
            .IsUnique();

        entity.HasIndex(e => e.UserId)
            .HasDatabaseName("IX_RefreshTokens_UserID");

        entity.HasIndex(e => e.ExpiresAt)
            .HasDatabaseName("IX_RefreshTokens_ExpiresAt");
    }
}

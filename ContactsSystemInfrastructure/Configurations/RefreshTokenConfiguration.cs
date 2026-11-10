using ContactsSystemInfrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ContactsSystemInfrastructure.Configurations;

public sealed class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> entity)
    {
        entity.ToTable("RefreshTokens");
        entity.HasKey(token => token.Id);
        entity.Property(token => token.TokenHash).HasMaxLength(64).IsRequired();
        entity.Property(token => token.UserId).HasMaxLength(450).IsRequired();
        entity.Property(token => token.ReplacedByTokenHash).HasMaxLength(64);
        entity.HasIndex(token => token.TokenHash).IsUnique();
        entity.HasIndex(token => new { token.UserId, token.ExpiresAtUtc });
        entity.HasOne(token => token.User)
            .WithMany(user => user.RefreshTokens)
            .HasForeignKey(token => token.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

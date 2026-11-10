
using ContactsSystemDomain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ContactsSystemInfrastructure.Configurations;

public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> entity)
    {
        entity.ToTable("Users");

        entity.HasKey(e => e.UserId);

        entity.Property(e => e.UserId)
            .HasColumnName("UserID");

        entity.Property(e => e.ContactId)
            .HasColumnName("ContactID");

        entity.Property(e => e.PasswordHash)
            .HasMaxLength(255)
            .IsRequired();

        entity.Property(e => e.Role)
            .HasMaxLength(50)
            .HasDefaultValue("User")
            .IsRequired();

        entity.Property(e => e.IsActive)
            .HasDefaultValue(true)
            .IsRequired();

        entity.HasIndex(e => e.ContactId)
            .IsUnique();

        entity.HasOne(e => e.ProfileContact)
            .WithOne()
            .HasForeignKey<User>(e => e.ContactId)
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasMany(e => e.RefreshTokens)
            .WithOne(e => e.User)
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

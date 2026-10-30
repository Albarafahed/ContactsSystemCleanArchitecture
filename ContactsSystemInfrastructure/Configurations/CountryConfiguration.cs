using ContactsSystemDomain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ContactsSystemInfrastructure.Configurations;

public class CountryConfiguration : IEntityTypeConfiguration<Country>
{
    public void Configure(EntityTypeBuilder<Country> entity)
    {
        entity.ToTable("Countries");

        entity.HasKey(e => e.CountryId);

        entity.Property(e => e.CountryId)
            .HasColumnName("CountryID");

        entity.Property(e => e.Code)
            .HasMaxLength(3);

        entity.Property(e => e.CountryName)
            .HasMaxLength(50);

        entity.Property(e => e.PhoneCode)
            .HasMaxLength(3);
    }
}
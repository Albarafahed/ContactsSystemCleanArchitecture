using ContactsSystemDomain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ContactsSystemInfrastructure.Configurations;

public sealed class ContactConfiguration : IEntityTypeConfiguration<Contact>
{
    public void Configure(EntityTypeBuilder<Contact> entity)
    {
        entity.ToTable("Contacts");

        entity.HasKey(e => e.ContactId)
            .HasName("PK__Contacts__5C6625BBE2F19656");

        entity.Property(e => e.ContactId)
            .HasColumnName("ContactID");

        entity.Property(e => e.Address)
            .HasMaxLength(200);

        entity.Property(e => e.CountryId)
            .HasColumnName("CountryID");

        entity.Property(e => e.DateOfBirth)
            .HasColumnType("datetime");

        entity.Property(e => e.Email)
            .HasMaxLength(50);

        entity.Property(e => e.FirstName)
            .HasMaxLength(50);

        entity.Property(e => e.ImagePath)
            .HasMaxLength(500);

        entity.Property(e => e.LastName)
            .HasMaxLength(50);

        entity.Property(e => e.Phone)
            .HasMaxLength(20);

        entity.HasOne(e => e.Country)
            .WithMany(c => c.Contacts)
            .HasForeignKey(e => e.CountryId)
            .OnDelete(DeleteBehavior.ClientSetNull);
    }
}

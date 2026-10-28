using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ContactsSystemInfrastructure.Models;

public partial class ContactsDbContext : DbContext
{
    public ContactsDbContext()
    {
    }

    public ContactsDbContext(DbContextOptions<ContactsDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Contact> Contacts { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=ContactsDB;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Contact>(entity =>
        {
            entity.HasKey(e => e.ContactId).HasName("PK__Contacts__5C6625BBE2F19656");

            entity.Property(e => e.ContactId).HasColumnName("ContactID");
            entity.Property(e => e.Address).HasMaxLength(200);
            entity.Property(e => e.CountryId).HasColumnName("CountryID");
            entity.Property(e => e.DateOfBirth).HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.ImagePath).HasMaxLength(500);
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.Phone).HasMaxLength(20);

            entity.HasOne(d => d.Country).WithMany(p => p.Contacts)
                .HasForeignKey(d => d.CountryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Contacts__Countr__398D8EEE");
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.HasKey(e => e.CountryId).HasName("PK__Countrie__10D160BFEAB15452");

            entity.Property(e => e.CountryId).HasColumnName("CountryID");
            entity.Property(e => e.Code).HasMaxLength(3);
            entity.Property(e => e.CountryName).HasMaxLength(50);
            entity.Property(e => e.PhoneCode).HasMaxLength(3);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

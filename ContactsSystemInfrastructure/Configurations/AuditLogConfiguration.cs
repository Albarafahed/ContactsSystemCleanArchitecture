using ContactsSystemDomain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ContactsSystemInfrastructure.Persistence.Configurations;

public class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
{
    public void Configure(EntityTypeBuilder<AuditLog> builder)
    {
        builder.ToTable("AuditLogs");

        builder.HasKey(x => x.AuditLogId);

        builder.Property(x => x.Action)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.EntityName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.EntityId)
            .HasMaxLength(100);

        builder.Property(x => x.Outcome)
            .IsRequired()
            .HasMaxLength(30);

        builder.Property(x => x.TimestampUtc)
            .IsRequired()
            .HasColumnType("datetime2");

        builder.Property(x => x.TraceId)
            .HasMaxLength(100);

        builder.HasIndex(x => x.UserId);

        builder.HasIndex(x => x.TimestampUtc);
    }
}
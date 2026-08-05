using ContentHub.Domain.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentHub.Persistence.Configurations;

public sealed class AuditLogConfiguration:IEntityTypeConfiguration<AuditLog>
{
    public void Configure(EntityTypeBuilder<AuditLog> builder)
    {
        builder.ToTable("AuditLogs");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Action)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.EntityName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.EntityId)
            .HasMaxLength(50);

        builder.Property(x => x.UserName)
            .HasMaxLength(256);

        builder.Property(x => x.FailureReason)
            .HasMaxLength(1000);

        builder.Property(x => x.IpAddress)
            .HasMaxLength(45);

        builder.Property(x=>x.TimestampUtc)
            .IsRequired();

        builder.Property(x => x.IsSuccessful)
            .IsRequired();


        builder.HasIndex(x => x.TimestampUtc);
        builder.HasIndex(x => x.UserId);
        builder.HasIndex(x => x.Action);
        builder.HasIndex(x => x.EntityName);
        builder.HasIndex(x => new { x.EntityName, x.EntityId });
    }
}

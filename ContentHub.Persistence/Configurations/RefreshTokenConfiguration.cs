using ContentHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentHub.Persistence.Configurations;

public sealed class RefreshTokenConfiguration:IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable("RefreshTokens");

        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.User)
            .WithMany(x => x.RefreshTokens)
            .HasForeignKey(x => x.UserId);

        builder.Property(x => x.Token)
            .IsRequired()
            .HasMaxLength(200);

        builder.HasIndex(x => x.Token)
            .IsUnique();

        builder.Property(x => x.ExpiresOnUtc)
            .IsRequired();

        builder.Property(x => x.CreatedOnUtc)
            .IsRequired();

        //builder.Property(x => x.RevokedOnUtc);

        builder.Property(x => x.UserId)
            .IsRequired();
    }
}

using System;
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;
public class JoinRequestConfiguration : IEntityTypeConfiguration<JoinRequest>
{
    public void Configure(EntityTypeBuilder<JoinRequest> builder)
    {
        builder.HasKey(j => j.Id);
        builder.Property(j => j.Id).HasMaxLength(36).IsRequired();
        builder.Property(j => j.UserId).HasMaxLength(36).IsRequired();
        builder.Property(j => j.OrganizationId).HasMaxLength(36).IsRequired();
        builder.Property(j => j.ResolverId).HasMaxLength(36);

        builder.HasQueryFilter(j => j.Organization.DeletedAt == null);

        builder.HasIndex(j => new { j.UserId, j.OrganizationId, j.Status })
               .HasFilter("\"Status\" = 'Pending'")
               .IsUnique();

        builder.Property(j => j.Status)
               .IsRequired()
               .HasConversion<string>()
               .HasMaxLength(20);

        builder.Property(j => j.RequestedAt).IsRequired();
    }
}

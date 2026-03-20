using System;
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

// Infrastructure/Configurations/OrgMemberConfiguration.cs
public class OrgMemberConfiguration : IEntityTypeConfiguration<OrganizationMember>
{
    public void Configure(EntityTypeBuilder<OrganizationMember> builder)
    {
        builder.HasKey(m => m.Id);
        builder.Property(m => m.Id).HasMaxLength(36).IsRequired();
        builder.Property(m => m.UserId).HasMaxLength(36).IsRequired();
        builder.Property(m => m.OrganizationId).HasMaxLength(36).IsRequired();

        builder.HasIndex(m => new { m.UserId, m.OrganizationId }).IsUnique();

        builder.HasQueryFilter(m => m.Organization.DeletedAt == null);

        builder.Property(m => m.Role)
               .IsRequired()
               .HasConversion<string>()
               .HasMaxLength(20);

        builder.Property(m => m.JoinedAt).IsRequired();
    }
}

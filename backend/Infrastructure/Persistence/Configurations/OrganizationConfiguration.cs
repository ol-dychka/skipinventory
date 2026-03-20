using System;
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;
public class OrganizationConfiguration : IEntityTypeConfiguration<Organization>
{
    public void Configure(EntityTypeBuilder<Organization> builder)
    {
        builder.HasKey(o => o.Id);
        builder.Property(o => o.Id).HasMaxLength(36).IsRequired();
        builder.Property(o => o.Name).HasMaxLength(200).IsRequired();
        builder.HasIndex(o => o.Name).IsUnique();
        builder.Property(o => o.CreatedAt).IsRequired();

        builder.HasQueryFilter(o => o.DeletedAt == null);

        builder.HasOne(o => o.Creator)
               .WithMany(u => u.CreatedOrganizations)
               .HasForeignKey(o => o.CreatorId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(o => o.Members)
               .WithOne(m => m.Organization)
               .HasForeignKey(m => m.OrganizationId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(o => o.JoinRequests)
               .WithOne(j => j.Organization)
               .HasForeignKey(j => j.OrganizationId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}

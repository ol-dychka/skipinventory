using System;
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        // Primary key
        builder.HasKey(p => p.Id);
        builder.Property(s => s.Id).HasMaxLength(36).IsRequired();
        builder.Property(s => s.OrganizationId).HasMaxLength(36).IsRequired();
        builder.Property(p => p.Sku).IsRequired().HasMaxLength(100);
        builder.Property(p => p.Name).IsRequired().HasMaxLength(255);
        builder.Property(p => p.Category).HasMaxLength(100);
        builder.Property(p => p.Vendor).IsRequired().HasMaxLength(255);
        builder.Property(p => p.Unit).HasConversion<string>().HasMaxLength(20);
        builder.Property(p => p.Currency).IsRequired().HasMaxLength(3);
        builder.Property(p => p.CostPrice).HasPrecision(18, 4);
        builder.Property(p => p.SalePrice).HasPrecision(18, 4);

        builder.HasQueryFilter(p => p.Organization.DeletedAt == null);

        builder.HasIndex(p => p.OrganizationId);

        builder.HasIndex(p => new { p.OrganizationId, p.Sku }).IsUnique();

        builder.HasIndex(p => new { p.OrganizationId, p.IsActive });
    }
}

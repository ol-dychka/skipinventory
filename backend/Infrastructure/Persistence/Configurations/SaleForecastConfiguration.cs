using System;
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class SaleForecastConfiguration : IEntityTypeConfiguration<SaleForecast>
{
    public void Configure(EntityTypeBuilder<SaleForecast> builder)
    {
        builder.HasKey(f => f.Id);
        builder.Property(s => s.Id).HasMaxLength(36).IsRequired();
        builder.Property(s => s.ProductId).HasMaxLength(36).IsRequired();
        builder.Property(s => s.OrganizationId).HasMaxLength(36).IsRequired();
        builder.Property(f => f.Sku).IsRequired().HasMaxLength(100);
        builder.Property(f => f.ModelVersion).IsRequired().HasMaxLength(20);
        // Restrict so deleting a product doesn't silently orphan forecasts
        builder
            .HasOne(f => f.Product)
            .WithMany()
            .HasForeignKey(f => f.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        // ── Indexes ──────────────────────────────────────────────────────────
        builder.HasIndex(f => f.OrganizationId);

        // Fetch latest forecast for a product — common query
        builder.HasIndex(f => new
        {
            f.OrganizationId,
            f.ProductId,
            f.ForecastStart,
        });

        // Fetch all forecasts for a given week across an org (dashboard view)
        builder.HasIndex(f => new { f.OrganizationId, f.ForecastStart });
    }
}

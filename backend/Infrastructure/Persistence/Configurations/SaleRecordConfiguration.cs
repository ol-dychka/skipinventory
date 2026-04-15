using System;
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class SaleRecordConfiguration : IEntityTypeConfiguration<SaleRecord>
{
    public void Configure(EntityTypeBuilder<SaleRecord> builder)
    {
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).HasMaxLength(36).IsRequired();
        builder.Property(s => s.OrganizationId).HasMaxLength(36).IsRequired();
        builder.Property(s => s.ProductId).HasMaxLength(36).IsRequired();
        builder.Property(s => s.Sku).IsRequired().HasMaxLength(100);

        // Relationship — restrict so you can't delete a product that has sales
        builder
            .HasOne(s => s.Product)
            .WithMany()
            .HasForeignKey(s => s.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        // ── Indexes ──────────────────────────────────────────────────────────
        builder.HasIndex(s => s.OrganizationId);

        // One record per product per day per org — enforced at DB level
        builder
            .HasIndex(s => new
            {
                s.OrganizationId,
                s.ProductId,
                s.Date,
            })
            .IsUnique();

        // For date-range dashboard queries ("show me this week")
        builder.HasIndex(s => new { s.OrganizationId, s.Date });
    }
}

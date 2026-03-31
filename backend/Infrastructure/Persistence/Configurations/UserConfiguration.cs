using System;
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);
        builder.Property(u => u.Id).HasMaxLength(36).IsRequired();
        builder.Property(u => u.Name).HasMaxLength(100).IsRequired();
        builder.Property(u => u.Email).HasMaxLength(256).IsRequired();
        builder.HasIndex(u => u.Email).IsUnique();
        builder.Property(u => u.PasswordHash).IsRequired();
        builder.Property(u => u.CreatedAt).IsRequired();

        builder
            .HasMany(u => u.Memberships)
            .WithOne(m => m.User)
            .HasForeignKey(m => m.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(u => u.PendingRequests)
            .WithOne(j => j.User)
            .HasForeignKey(j => j.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(u => u.ResolvedRequests)
            .WithOne(j => j.Resolver)
            .HasForeignKey(j => j.ResolverId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(u => u.RefreshTokens)
            .WithOne(r => r.User)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class ChatMessageConfiguration : IEntityTypeConfiguration<ChatMessage>
{
    public void Configure(EntityTypeBuilder<ChatMessage> builder)
    {
        builder.HasKey(cm => cm.Id);
        builder.Property(cm => cm.Id).HasMaxLength(36).IsRequired();
        builder.Property(cm => cm.RoomId).HasMaxLength(36).IsRequired();
        builder.Property(cm => cm.SenderId).HasMaxLength(36).IsRequired();
        builder.Property(cm => cm.Content).HasMaxLength(2000).IsRequired();
        builder.Property(cm => cm.SentAt).IsRequired();

        builder.HasIndex(j => j.RoomId);

        builder.HasOne(m => m.Sender).WithMany().HasForeignKey(m => m.SenderId);
    }
}

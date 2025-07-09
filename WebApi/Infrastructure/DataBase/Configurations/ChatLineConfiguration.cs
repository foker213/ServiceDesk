using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServiceDesk.Domain.Database.Models;

namespace ServiceDesk.Infrastructure.Database.Configurations;

internal sealed class ChatLineConfiguration : IEntityTypeConfiguration<ChatLine>
{
    public void Configure(EntityTypeBuilder<ChatLine> builder)
    {
        builder.ToTable("ChatLines");

        builder.HasKey(x => x.Id);

        builder.Property(fw => fw.Id).ValueGeneratedOnAdd();

        builder.HasOne(x => x.Chat)
            .WithMany()
            .HasForeignKey(x => x.ChatId);

        builder.Property(x => x.ChatId)
            .HasComment("Прикрепленный чат");

        builder.Property(x => x.Message)
            .HasComment("Сообщение");
    }
}

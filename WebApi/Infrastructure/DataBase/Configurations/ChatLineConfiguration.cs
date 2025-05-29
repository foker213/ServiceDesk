using Domain.DataBase.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.DataBase.Configurations;

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

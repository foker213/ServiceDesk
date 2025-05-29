using Domain.DataBase.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.DataBase.Configurations;

internal sealed class ChatConfiguration : IEntityTypeConfiguration<Chat>
{
    public void Configure(EntityTypeBuilder<Chat> builder)
    {
        builder.ToTable("Chats");

        builder.HasKey(x => x.Id);

        builder.Property(fw => fw.Id).ValueGeneratedOnAdd();

        builder.HasOne(x => x.ExternalUser)
            .WithOne(x => x.Chat)
            .HasForeignKey<Chat>(x => x.ExternalUserId);

        builder.Property(x => x.ExternalUserId)
            .HasComment("Прикрепленный чат");
    }
}

using Domain.DataBase.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.DataBase.Configurations;

internal sealed class RequestConfiguration : IEntityTypeConfiguration<Request>
{
    public void Configure(EntityTypeBuilder<Request> builder)
    {
        builder.ToTable("Requests");

        builder.HasKey(x => x.Id);

        builder.Property(fw => fw.Id).ValueGeneratedOnAdd();

        builder.HasOne(x => x.User)
            .WithOne(x => x.Request)
            .HasForeignKey<Request>(x => x.UserId);

        builder.Property(x => x.UserId)
            .HasComment("Назначенный пользователь");

        builder.Property(x => x.Status)
            .HasConversion<string>()
            .HasComment("Статус");

        builder.HasOne(x => x.Chat)
            .WithOne(x => x.Request)
            .HasForeignKey<Request>(x => x.ChatId);

        builder.Property(x => x.ChatId)
            .HasComment("Прекрепленный чат");

        builder.Property(x => x.DateStartRequest)
            .HasComment("Заявка начата");

        builder.Property(x => x.DateEndRequest)
            .HasComment("Заявка завершена");
    }
}

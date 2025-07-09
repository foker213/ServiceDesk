using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServiceDesk.Domain.Database.Models;

namespace ServiceDesk.Infrastructure.Database.Configurations;

internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(x => x.Id);

        builder.Property(fw => fw.Id).ValueGeneratedOnAdd();

        builder.Property(u => u.UserName)
               .HasComment("Логин");

        builder.Property(u => u.PasswordHash)
               .HasComment("Пароль");

        builder.Property(u => u.Email)
               .HasComment("Email");

        builder.Property(u => u.Name)
               .HasMaxLength(100)
               .HasComment("ФИО");

        builder.Property(u => u.PhoneNumber)
               .HasComment("Телефон");

        builder.Property(u => u.Blocked)
               .HasComment("Заблокирован");

        builder.Property(u => u.BlockedAt)
               .HasComment("Время блокировки");

        builder.Property(u => u.BlockedReason)
               .HasComment("Причина блокировки");

        builder.Property(u => u.LastLogonTime)
               .HasComment("Время последнего входа");
    }
}

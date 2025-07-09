using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServiceDesk.Domain.Database.Models;

namespace ServiceDesk.Infrastructure.Database.Configurations;

internal sealed class ExternalUserConfiguration : IEntityTypeConfiguration<ExternalUser>
{
    public void Configure(EntityTypeBuilder<ExternalUser> builder)
    {
        builder.ToTable("ExternalUsers");

        builder.HasKey(x => x.Id);

        builder.Property(fw => fw.Id).ValueGeneratedOnAdd();

        builder.Property(x => x.Name)
            .HasComment("ФИО");

        builder.Property(x => x.NumberPhone)
            .HasMaxLength(20)
            .HasComment("Номер телефона");

        builder.Property(x => x.Email)
            .HasMaxLength(60)
            .HasComment("Электронный адрес");
    }
}
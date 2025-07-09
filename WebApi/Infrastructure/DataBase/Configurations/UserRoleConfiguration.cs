using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServiceDesk.Domain.Database.Models;

namespace ServiceDesk.Infrastructure.Database.Configurations;

internal sealed class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        builder.ToTable("UserRoles");

        builder.HasKey(x => x.Id);

        builder.Property(fw => fw.Id).ValueGeneratedOnAdd();
    }
}

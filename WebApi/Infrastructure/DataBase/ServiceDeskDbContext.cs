using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ServiceDesk.Domain.Database.Models;

namespace ServiceDesk.Infrastructure.Database;

public class ServiceDeskDbContext(DbContextOptions<ServiceDeskDbContext> options)
    : IdentityDbContext<User, UserRole, int>(options)

{
    public DbSet<Request> Requests => Set<Request>();
    public DbSet<Chat> Chats => Set<Chat>();
    public DbSet<ChatLine> ChatLines => Set<ChatLine>();
    public DbSet<ExternalUser> ExternalUsers => Set<ExternalUser>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }

}

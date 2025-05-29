using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DataBase;

public class ServiceDeskDbContextFactory : IDesignTimeDbContextFactory<ServiceDeskDbContext>
{
    public ServiceDeskDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ServiceDeskDbContext>();
        optionsBuilder.UseNpgsql(
            "Server=127.0.0.1;Port=5432;Database=ServiceDesk;User Id=guest;Password=guest;Pooling=true;"
            );
        return new ServiceDeskDbContext(optionsBuilder.Options);
    }
}


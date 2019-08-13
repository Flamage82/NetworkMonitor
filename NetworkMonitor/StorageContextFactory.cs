using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;

namespace NetworkMonitor
{
    public class StorageContextFactory: IDesignTimeDbContextFactory<StorageContext>
    {
        public StorageContext CreateDbContext(string[] args)
        {
            var configuration = Program.GetConfiguration();
            var serviceProvider = Program.BuildServiceProvider(configuration);
            return serviceProvider.GetRequiredService<StorageContext>();
        }
    }
}
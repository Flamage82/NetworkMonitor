using Microsoft.EntityFrameworkCore;

namespace NetworkMonitor
{
    public class StorageContext : DbContext
    {
        public StorageContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
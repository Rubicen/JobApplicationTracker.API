using DatabaseContext.Entities;
using Microsoft.EntityFrameworkCore;

namespace DatabaseContext
{
    public class AppDatabaseContext : DbContext
    {
        public AppDatabaseContext(DbContextOptions<AppDatabaseContext> options)
            : base(options)
        {
        }

        public DbSet<ApplicationEntity> Applications { get; set; }
    }
}
```csharp
using Microsoft.EntityFrameworkCore;
using System;

namespace Data
{
    public class AppDbContext : DbContext
    {
        private readonly string _connectionString;

        public AppDbContext(DbContextOptions<AppDbContext> options) 
            : base(options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            var connectionString = options.FindExtension<SqlServerDbContextOptionsExtensions>().ConnectionString;
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentException("Connection string is empty or null", nameof(options));
            }

            _connectionString = connectionString;
        }

        public DbSet<MyEntity> MyEntities { get; set; }
    }

    public class MyEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
```
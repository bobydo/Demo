using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Demo.Infrastructure
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            // Use your connection string here (adjust as needed)
            optionsBuilder.UseSqlite("Data Source=../Database/Demo.db");
            return new AppDbContext(optionsBuilder.Options);
        }
    }
}

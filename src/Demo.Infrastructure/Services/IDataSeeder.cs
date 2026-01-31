using System.Threading;
using System.Threading.Tasks;

namespace Demo.Infrastructure.Services
{
    public interface IDataSeeder
    {
        Task SeedAsync(CancellationToken cancellationToken = default);
    }
}

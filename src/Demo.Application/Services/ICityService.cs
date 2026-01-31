using Demo.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Demo.Application.Services
{
    public interface ICityService
    {
        Task<IEnumerable<City>> GetAllAsync();
        Task<City> GetByIdAsync(int id);
        Task<City> CreateAsync(City city);
        Task<City> UpdateAsync(City city);
        Task<bool> DeleteAsync(int id);
    }
}

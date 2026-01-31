using Demo.Infrastructure; 
using Demo.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks; 

namespace Demo.Application.Services
{
    public class CityService : ICityService
    {
        private readonly AppDbContext _context;

        public CityService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<City>> GetAllAsync()
        {
            return await _context.Set<City>().ToListAsync();
        }

        public async Task<City> GetByIdAsync(int id)
        {
            return await _context.Set<City>().FindAsync(id);
        }

        public async Task<City> CreateAsync(City city)
        {
            _context.Set<City>().Add(city);
            await _context.SaveChangesAsync();
            return city;
        }

        public async Task<City> UpdateAsync(City city)
        {
            _context.Set<City>().Update(city);
            await _context.SaveChangesAsync();
            return city;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var city = await _context.Set<City>().FindAsync(id);
            if (city == null) return false;
            _context.Set<City>().Remove(city);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

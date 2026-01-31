using Demo.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Demo.Infrastructure;

namespace Demo.Application.Services
{
    public class EventCodeDefinitionService : IEventCodeDefinitionService
    {
        private readonly AppDbContext _context;
        public EventCodeDefinitionService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<EventCodeDefinition>> GetAllAsync()
        {
            return await _context.Set<EventCodeDefinition>().ToListAsync();
        }

        public async Task<EventCodeDefinition> GetByCodeAsync(string eventCode)
        {
            return await _context.Set<EventCodeDefinition>().FindAsync(eventCode);
        }

        public async Task<EventCodeDefinition> CreateAsync(EventCodeDefinition def)
        {
            _context.Set<EventCodeDefinition>().Add(def);
            await _context.SaveChangesAsync();
            return def;
        }

        public async Task<EventCodeDefinition> UpdateAsync(EventCodeDefinition def)
        {
            _context.Set<EventCodeDefinition>().Update(def);
            await _context.SaveChangesAsync();
            return def;
        }

        public async Task<bool> DeleteAsync(string eventCode)
        {
            var def = await _context.Set<EventCodeDefinition>().FindAsync(eventCode);
            if (def == null) return false;
            _context.Set<EventCodeDefinition>().Remove(def);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

using Demo.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Demo.Infrastructure;

namespace Demo.Application.Services
{
    public class EventService : IEventService
    {
        private readonly AppDbContext _context;
        public EventService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Event>> GetAllAsync()
        {
            return await _context.Set<Event>().ToListAsync();
        }

        public async Task<Event> GetByIdAsync(int id)
        {
            return await _context.Set<Event>().FindAsync(id);
        }

        public async Task<Event> CreateAsync(Event evt)
        {
            _context.Set<Event>().Add(evt);
            await _context.SaveChangesAsync();
            return evt;
        }

        public async Task<Event> UpdateAsync(Event evt)
        {
            _context.Set<Event>().Update(evt);
            await _context.SaveChangesAsync();
            return evt;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var evt = await _context.Set<Event>().FindAsync(id);
            if (evt == null) return false;
            _context.Set<Event>().Remove(evt);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

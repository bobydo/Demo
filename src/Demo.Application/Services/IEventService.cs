using Demo.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Demo.Application.Services
{
    public interface IEventService
    {
        Task<IEnumerable<Event>> GetAllAsync();
        Task<Event> GetByIdAsync(int id);
        Task<Event> CreateAsync(Event evt);
        Task<Event> UpdateAsync(Event evt);
        Task<bool> DeleteAsync(int id);
    }
}

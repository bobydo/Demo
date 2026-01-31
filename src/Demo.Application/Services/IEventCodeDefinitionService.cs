using Demo.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Demo.Application.Services
{
    public interface IEventCodeDefinitionService
    {
        Task<IEnumerable<EventCodeDefinition>> GetAllAsync();
        Task<EventCodeDefinition> GetByCodeAsync(string eventCode);
        Task<EventCodeDefinition> CreateAsync(EventCodeDefinition def);
        Task<EventCodeDefinition> UpdateAsync(EventCodeDefinition def);
        Task<bool> DeleteAsync(string eventCode);
    }
}

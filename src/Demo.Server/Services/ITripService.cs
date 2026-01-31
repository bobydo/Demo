using System.Collections.Generic;
using System.Threading.Tasks;
using Demo.Server.Models;

namespace Demo.Server.Services
{
    public interface ITripService
    {
        Task<List<Trip_DTO>> GetTripsAsync();
    }
}

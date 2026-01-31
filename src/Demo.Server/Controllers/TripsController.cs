using Microsoft.AspNetCore.Mvc;
using Demo.Server.Services;
using Demo.Server.Domain.API.DTOs;

namespace Demo.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TripsController : ControllerBase
    {
        private readonly ITripService _tripService;

        public TripsController(ITripService tripService)
        {
            _tripService = tripService;
        }

        [HttpGet]
        public async Task<IActionResult> GetTrips()
        {
            var trips = await _tripService.GetTripsAsync();
            return Ok(trips);
        }
    }
}

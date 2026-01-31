using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Caching.Memory;
using Demo.Application.Services;
using Demo.Server.Common;
using Demo.Server.Models;
using System.Threading.Tasks;

namespace Demo.Server.Services
{
    public class TripService : ITripService
    {

        private readonly IMemoryCache _cache;
        private readonly ICityService _cityService;
        private readonly IEventService _eventService;
        private readonly IEventCodeDefinitionService _eventCodeDefinitionService;
        private const string CacheKey = "trips";

        public TripService(
            IMemoryCache cache,
            ICityService cityService,
            IEventService eventService,
            IEventCodeDefinitionService eventCodeDefinitionService)
        {
            _cache = cache;
            _cityService = cityService;
            _eventService = eventService;
            _eventCodeDefinitionService = eventCodeDefinitionService;
        }

        public async Task<List<Trip_DTO>> GetTripsAsync()
        {
            var trips = await _cache.GetOrCreateAsync(CacheKey, async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10);
                var allEvents = (await _eventService.GetAllAsync()).ToList();
                // Filter to only events for EquipmentId groups that have both W and Z, and order by EquipmentId and Event_Time
                var validEvents = allEvents
                    .GroupBy(e => e.EquipmentId)
                    .Where(g => g.Any(ev => ev.EventCode == "W") && g.Any(ev => ev.EventCode == "Z"))
                    .SelectMany(g => g.OrderBy(ev => ev.Event_Time))
                    .OrderBy(e => e.EquipmentId)
                    .ThenBy(e => e.Event_Time)
                    .ToList();

                var cities = (await _cityService.GetAllAsync()).ToDictionary(c => c.CityId, c => c.Name);
                var eventCodeDefs = (await _eventCodeDefinitionService.GetAllAsync()).ToDictionary(d => d.EventCode, d => d.Description);

                var tripsList = new List<Trip_DTO>();
                string? lastEquipmentId = null;
                Trip_DTO? currentTrip = null;
                foreach (var e in validEvents)
                {
                    if (e.EventCode == "W")
                    {
                        var originCity = cities.ContainsKey(e.CityId) ? cities[e.CityId] : Constraints.Undefined;
                        var eventCodeDescription = eventCodeDefs.ContainsKey(e.EventCode) ? eventCodeDefs[e.EventCode] : Constraints.Undefined;
                        currentTrip = new Trip_DTO
                        {
                            EquipmentId = e.EquipmentId,
                            EquipmentDescription = eventCodeDescription,
                            OriginCityName = originCity,
                            DestinationCityName = Constraints.Undefined, // set default, will update on Z
                            StartUtc = e.Event_Time
                        };
                        lastEquipmentId = e.EquipmentId;
                    }
                    else if (e.EventCode == "Z" && currentTrip != null && e.EquipmentId == lastEquipmentId)
                    {
                        currentTrip.EndUtc = e.Event_Time;
                        currentTrip.DestinationCityName = cities.ContainsKey(e.CityId) ? cities[e.CityId] : Constraints.Undefined;
                        currentTrip.TotalTripHours = (currentTrip.EndUtc - currentTrip.StartUtc).TotalHours;
                        tripsList.Add(currentTrip);
                        currentTrip = null; // Reset after adding
                        lastEquipmentId = null;
                    }
                    // else: ignore events that are not trip start/end
                }
                return tripsList;
            });
            return trips ?? new List<Trip_DTO>();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Demo.Domain.Entities;
using Demo.Server.Services;
using Demo.Application.Services;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using Xunit;

namespace Demo.Tests.Validation
{
    public class TripServiceTests
    {
        [Fact]
        public async Task GetTripsAsync_ReturnsTrips_WhenEventsAreValid()
        {
            // Arrange
            var cache = new MemoryCache(new MemoryCacheOptions());
            var cityServiceMock = new Mock<ICityService>();
            var eventServiceMock = new Mock<IEventService>();
            var eventCodeDefServiceMock = new Mock<IEventCodeDefinitionService>();


            var cities = new List<City> {
                new City { CityId = 1, Name = "CityOne", Time_Zone = "UTC", Events = new List<Event>() },
                new City { CityId = 2, Name = "CityTwo", Time_Zone = "UTC", Events = new List<Event>() }
            };
            cityServiceMock.Setup(s => s.GetAllAsync()).Returns(Task.FromResult<IEnumerable<City>>(cities));

            var eventCodeDefs = new List<EventCodeDefinition> {
                new EventCodeDefinition { EventCode = "W", Description = "Start", Long_Description = "Trip Start", Events = new List<Event>() },
                new EventCodeDefinition { EventCode = "Z", Description = "End", Long_Description = "Trip End", Events = new List<Event>() }
            };
            eventCodeDefServiceMock.Setup(s => s.GetAllAsync()).Returns(Task.FromResult<IEnumerable<EventCodeDefinition>>(eventCodeDefs));

            var now = DateTime.UtcNow;
            var events = new List<Event> {
                new Event {
                    Id = 1,
                    EquipmentId = "E1",
                    EventCode = "W",
                    CityId = 1,
                    Event_Time = now,
                    City = cities[0],
                    EventCodeDefinition = eventCodeDefs[0]
                },
                new Event {
                    Id = 2,
                    EquipmentId = "E1",
                    EventCode = "Z",
                    CityId = 2,
                    Event_Time = now.AddHours(2),
                    City = cities[1],
                    EventCodeDefinition = eventCodeDefs[1]
                }
            };
            eventServiceMock.Setup(s => s.GetAllAsync()).Returns(Task.FromResult<IEnumerable<Event>>(events));

            var service = new TripService(cache, cityServiceMock.Object, eventServiceMock.Object, eventCodeDefServiceMock.Object);

            // Act
            var trips = await service.GetTripsAsync();

            // Assert
            Assert.Single(trips);
            var trip = trips[0];
            Assert.Equal("E1", trip.EquipmentId);
            Assert.Equal("Start", trip.EquipmentDescription);
            Assert.Equal("CityOne", trip.OriginCityName);
            Assert.Equal("CityTwo", trip.DestinationCityName);
            Assert.Equal(2, trip.TotalTripHours);
        }

        [Fact]
        public async Task GetTripsAsync_ReturnsEmpty_WhenNoValidEvents()
        {
            // Arrange
            var cache = new MemoryCache(new MemoryCacheOptions());
            var cityServiceMock = new Mock<ICityService>();
            var eventServiceMock = new Mock<IEventService>();
            var eventCodeDefServiceMock = new Mock<IEventCodeDefinitionService>();

            cityServiceMock.Setup(s => s.GetAllAsync()).Returns(Task.FromResult<IEnumerable<City>>(new List<City>()));
            eventCodeDefServiceMock.Setup(s => s.GetAllAsync()).Returns(Task.FromResult<IEnumerable<EventCodeDefinition>>(new List<EventCodeDefinition>()));
            eventServiceMock.Setup(s => s.GetAllAsync()).Returns(Task.FromResult<IEnumerable<Event>>(new List<Event>()));

            var service = new TripService(cache, cityServiceMock.Object, eventServiceMock.Object, eventCodeDefServiceMock.Object);

            // Act
            var trips = await service.GetTripsAsync();

            // Assert
            Assert.Empty(trips);
        }
    }
}

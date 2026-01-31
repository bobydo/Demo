
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Demo.Domain.Entities;
using System.ComponentModel;

namespace Demo.Infrastructure.Services
{
    public class CsvDataSeeder : IDataSeeder
    {
        private readonly AppDbContext _db;
        private readonly IConfiguration _config;
        private readonly ILogger<CsvDataSeeder> _logger;

        // Serilog’s {SourceContext} property refers to the class name because Microsoft.Extensions.Logging (used by ILogger<T>) 
        // automatically sets the SourceContext to the fully qualified name of the type T when you inject ILogger<T>. 
        public CsvDataSeeder(AppDbContext db, IConfiguration config, ILogger<CsvDataSeeder> logger)
        {
            _db = db;
            _config = config;
            _logger = logger;
        }

        public async Task SeedAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                // Get data directory from config, environment variable, or use default relative path
                var dataDir = _config["DataDir"];
                if (string.IsNullOrEmpty(dataDir))
                {
                    dataDir = Environment.GetEnvironmentVariable("DATA_DIR");
                }
                if (string.IsNullOrEmpty(dataDir))
                {
                    _logger.LogWarning("DataDir not configured in appsettings or environment variable. Using default relative path.");
                    //The .. segments move up four directories, so from bin/Debug/net8.0/ to src/.
                    dataDir = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "Data");
                }
                _logger.LogInformation($"Seeding from {dataDir}");
                try
                {
                    await SeedCities(dataDir, cancellationToken);
                    await Seed_EventCodeDefinitions(dataDir, cancellationToken);
                    await Seed_Events(dataDir, cancellationToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error seeding from CSV.");
                    throw;
                }

            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Critical error during CSV data seeding. Migration aborted.");
                throw;
            }
        }

        private async Task<bool> IsDatabaseSeededOrFileExists<TEntity>(
            string filePath,
            DbSet<TEntity> dbSet)
            where TEntity : class
        {
            if (!File.Exists(filePath)) {
                _logger.LogInformation($"{filePath} does not exist.");
                return false;
            }
            if (await dbSet.AnyAsync())
            {
                _logger.LogInformation($"{typeof(TEntity).Name} already seeded.");
                return false;
            }
            return true;
        }

        private async Task SeedCities(string dataDir, CancellationToken cancellationToken)
        {
            var citiesPath = Path.Combine(dataDir, "canadian_cities.csv");
            if(!await IsDatabaseSeededOrFileExists(citiesPath, _db.Cities))
                return;
            try
            {
                var lines = await File.ReadAllLinesAsync(citiesPath, cancellationToken);
                foreach (var line in lines.Skip(1)) // skip header
                {
                    var cols = line.Split(',');
                    if (cols.Length >= 3)
                        _ = _db.Cities.Add(new Domain.Entities.City
                        {
                            CityId = int.TryParse(cols[0], out var cityId) ? cityId : 0,
                            Name = cols[1],
                            Time_Zone = cols[2]
                        });
                }
                await _db.SaveChangesAsync(cancellationToken);
                _logger.LogInformation("Seeded Cities from CSV.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error seeding Cities from CSV.");
                throw;
            }
        }
        private async Task Seed_Events(string dataDir, CancellationToken cancellationToken)
        {
            var eventsPath = Path.Combine(dataDir, "equipment_events.csv");
            if(!await IsDatabaseSeededOrFileExists(eventsPath, _db.Events))
                return;
            try{
                var lines = await File.ReadAllLinesAsync(eventsPath, cancellationToken);
                foreach (var line in lines.Skip(1)) // skip header
                {
                    var cols = line.Split(',');
                    if (cols.Length >= 4)
                    {
                        DateTime eventTime;
                        if (!DateTime.TryParse(cols[2], out eventTime))
                        {
                            eventTime = DateTime.MinValue;
                        }
                        eventTime = eventTime.Kind == DateTimeKind.Utc ? eventTime : eventTime.ToUniversalTime();
                        _ = _db.Events.Add(new Domain.Entities.Event
                        {
                            EquipmentId = cols[0],
                            EventCode = cols[1],
                            Event_Time = eventTime,
                            CityId = int.TryParse(cols[3], out var cityId) ? cityId : 0
                        });
                    }
                }
                await _db.SaveChangesAsync(cancellationToken);
                _logger.LogInformation("Seeded Events from CSV.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error seeding Events from CSV.");
                throw;
            }

        }

        private async Task Seed_EventCodeDefinitions(string dataDir, CancellationToken cancellationToken)
        {
            // Seed EventCodeDefinitions
            var codesPath = Path.Combine(dataDir, "event_code_definitions.csv");
            if(!await IsDatabaseSeededOrFileExists(codesPath, _db.EventCodeDefinitions))
                return;
            try
            {
                var lines = await File.ReadAllLinesAsync(codesPath, cancellationToken);
                foreach (var line in lines.Skip(1))
                {
                    var cols = line.Split(',');
                    if (cols.Length >= 3)
                    {
                        _db.EventCodeDefinitions.Add(new Domain.Entities.EventCodeDefinition
                        {
                            EventCode = cols[0],
                            Description = cols[1],
                            Long_Description = cols[2]
                        });
                    }
                }
                await _db.SaveChangesAsync(cancellationToken);
                _logger.LogInformation("Seeded EventCodeDefinitions from CSV.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error seeding EventCodeDefinitions from CSV.");
                throw;
            }
        }
    }
}

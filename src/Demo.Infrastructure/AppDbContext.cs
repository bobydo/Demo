using Demo.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Demo.Infrastructure
{
    public class AppDbContext : DbContext
    {
        public DbSet<Event> Events { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<EventCodeDefinition> EventCodeDefinitions { get; set; }


        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            // Event.EventCode is a foreign key to EventCodeDefinition.EventCode
            modelBuilder.Entity<Event>()
                .HasOne(e => e.EventCodeDefinition)
                .WithMany()
                .HasForeignKey(e => e.EventCode);

            // Event.CityId is a foreign key to City.CityId
            modelBuilder.Entity<Event>()
                .HasOne(e => e.City)
                .WithMany(c => c.Events)
                .HasForeignKey(e => e.CityId)
                .HasPrincipalKey(c => c.CityId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

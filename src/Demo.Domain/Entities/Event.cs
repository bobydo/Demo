using System;
using System.ComponentModel.DataAnnotations;
namespace Demo.Domain.Entities
{
    public class Event
    {
        [Key]
        public int Id { get; set; }
        public string EquipmentId { get; set; }
        public string EventCode { get; set; }
        public DateTime Event_Time { get; set; }
        public int CityId { get; set; }
        public City City { get; set; }
        public EventCodeDefinition EventCodeDefinition { get; set; }
    }
}

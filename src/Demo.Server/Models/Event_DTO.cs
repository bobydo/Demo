using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Demo.Server.Models
{
    public class Event_DTO
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public required string EquipmentId { get; set; }

        [Required]
        public required string EventCode { get; set; }

        [Required]
        public DateTime Event_Time { get; set; }

        [Required]
        public int CityId { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public required City_DTO City { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public required EventCodeDefinition_DTO EventCodeDefinition { get; set; }
    }
}

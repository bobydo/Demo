using System;

namespace Demo.Server.Models
{
    public class City_DTO
    {
        public int CityId { get; set; }
        public required string Name { get; set; }
        public required string Time_Zone { get; set; }
    }
}

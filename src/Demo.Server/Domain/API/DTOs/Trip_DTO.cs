namespace Demo.Server.Domain.API.DTOs
{
    public class Trip_DTO
    {
        public string EquipmentId { get; set; } = string.Empty;
        public required string EquipmentDescription { get; set; }
        public required string OriginCityName { get; set; }
        public required string DestinationCityName { get; set; }
        public DateTime StartUtc { get; set; }
        public DateTime EndUtc { get; set; }
        public double TotalTripHours { get; set; }
    }
}

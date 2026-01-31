using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace Demo.Domain.Entities
{
    public class City
    {
        [Key]
        public int CityId { get; set; }
        public string Name { get; set; }
        public string Time_Zone { get; set; }
        public ICollection<Event> Events { get; set; }
    }
}

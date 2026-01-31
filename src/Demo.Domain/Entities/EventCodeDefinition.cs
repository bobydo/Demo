using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace Demo.Domain.Entities
{
	public class EventCodeDefinition
	{
		[Key]
		public string EventCode { get; set; }
		public string Description { get; set; }
		public string Long_Description { get; set; }
		public ICollection<Event> Events { get; set; }
	}
}

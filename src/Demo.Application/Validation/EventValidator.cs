using System;
using FluentValidation;
using Demo.Domain.Entities;

namespace Demo.Application.Validation
{
    public class EventValidator : AbstractValidator<Event>
    {
        public EventValidator()
        {
            RuleFor(x => x.EquipmentId)
                .NotEmpty().WithMessage("EquipmentId is required.");

            RuleFor(x => x.EventCode)
                .NotEmpty().WithMessage("EventCode is required.");

            RuleFor(x => x.Event_Time)
                .NotEmpty().WithMessage("Event_Time must be provided.");

            RuleFor(x => x.CityId)
                .GreaterThan(0).WithMessage("CityId must be a positive integer.");

            // TODO: Add advanced business rules (e.g., event sequence, duplicate detection)
        }
    }
}

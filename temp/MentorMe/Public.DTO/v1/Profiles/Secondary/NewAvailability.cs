using Domain.Enums;

namespace Public.DTO.v1.Profiles.Secondary;

public class NewAvailability
{
    public EAvailabilityDayOfTheWeek DayOfTheWeek { get; set; }
    public string FromHours { get; set; } = default!;
    public string ToHours { get; set; } = default!;
}
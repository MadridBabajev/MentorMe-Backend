using Domain.Enums;

namespace Public.DTO.v1.Profiles.Secondary;

public class Availability
{
    public Guid Id { get; set; }
    public TimeSpan FromHours { get; set; }
    public TimeSpan ToHours { get; set; }
    public EAvailabilityDayOfTheWeek DayOfTheWeek { get; set; }
}

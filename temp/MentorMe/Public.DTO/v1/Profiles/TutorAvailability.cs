using Domain.Enums;

namespace Public.DTO.v1.Profiles;

public class TutorAvailability
{
    public TimeSpan FromHours { get; set; }
    public TimeSpan ToHours { get; set; }
    public EAvailabilityDayOfTheWeek DayOfTheWeek { get; set; }
}
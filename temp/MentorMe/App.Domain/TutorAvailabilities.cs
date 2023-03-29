using Base.DomainEntity;
using Domain.Enums;

namespace Domain;

public class TutorAvailability: BaseDomainEntity
{
    public DateTime FromHours { get; set; }
    public DateTime ToHours { get; set; }
    public EAvailabilityDayOfTheWeak DayOfTheWeak { get; set; }
}

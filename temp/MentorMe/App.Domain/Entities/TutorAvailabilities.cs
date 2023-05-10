using Base.DomainEntity;
using Domain.Enums;

namespace Domain.Entities;

public class TutorAvailability: BaseDomainEntity
{
    public TimeSpan FromHours { get; set; }
    public TimeSpan ToHours { get; set; }
    public EAvailabilityDayOfTheWeak DayOfTheWeak { get; set; }
    
    // nav
    public Guid TutorId { get; set; }
    public Tutor? Tutor { get; set; }
}

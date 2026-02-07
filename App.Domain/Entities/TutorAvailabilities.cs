using Base.DAL.Contracts;
using Base.DomainEntity;
using Domain.Enums;

namespace Domain.Entities;

public class TutorAvailability: BaseDomainEntity, IDomainEntityId
{
    public TimeSpan FromHours { get; set; }
    public TimeSpan ToHours { get; set; }
    public EAvailabilityDayOfTheWeek DayOfTheWeek { get; set; }
    
    // nav
    public Guid TutorId { get; set; }
    public Tutor? Tutor { get; set; }
}

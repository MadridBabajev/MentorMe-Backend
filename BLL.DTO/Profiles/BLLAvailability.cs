using Base.DAL.Contracts;
using Domain.Enums;

namespace BLL.DTO.Profiles;

public class BLLAvailability: IDomainEntityId
{
    public Guid Id { get; set; }
    public TimeSpan FromHours { get; set; }
    public TimeSpan ToHours { get; set; }
    public EAvailabilityDayOfTheWeek DayOfTheWeek { get; set; }
}
using Domain.Enums;

namespace Public.DTO.v1.Profiles.Secondary;

/// <summary>
/// Represents the availability of a tutor for a specific day of the week and time range.
/// </summary>
public class Availability
{
    /// <summary>
    /// Gets or sets the unique identifier of the availability.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the starting time of the availability.
    /// </summary>
    public TimeSpan FromHours { get; set; }

    /// <summary>
    /// Gets or sets the ending time of the availability.
    /// </summary>
    public TimeSpan ToHours { get; set; }

    /// <summary>
    /// Gets or sets the day of the week for which the availability is defined.
    /// </summary>
    public EAvailabilityDayOfTheWeek DayOfTheWeek { get; set; }
}
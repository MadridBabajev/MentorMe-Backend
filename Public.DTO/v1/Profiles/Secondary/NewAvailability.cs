using Domain.Enums;

namespace Public.DTO.v1.Profiles.Secondary;

/// <summary>
/// Represents a new availability to be added for a tutor.
/// </summary>
public class NewAvailability
{
    /// <summary>
    /// Gets or sets the day of the week for the new availability.
    /// </summary>
    public EAvailabilityDayOfTheWeek DayOfTheWeek { get; set; }

    /// <summary>
    /// Gets or sets the starting time of the new availability.
    /// </summary>
    public string FromHours { get; set; } = default!;

    /// <summary>
    /// Gets or sets the ending time of the new availability.
    /// </summary>
    public string ToHours { get; set; } = default!;
}
namespace Public.DTO.v1.Profiles.Secondary;

/// <summary>
/// Represents a request to remove an availability.
/// </summary>
public class RemoveAvailability
{
    /// <summary>
    /// Gets or sets the unique identifier of the availability to be removed.
    /// </summary>
    public Guid AvailabilityId { get; set; }
}
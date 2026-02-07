using System.ComponentModel.DataAnnotations.Schema;
using Public.DTO.v1.Profiles.Secondary;

namespace Public.DTO.v1.Profiles;

/// <summary>
/// Represents the profile data for a tutor.
/// Inherits from the base profile data.
/// </summary>
public class TutorProfile : BaseProfileData
{
    /// <summary>
    /// Gets or sets the hourly rate of the tutor.
    /// </summary>
    [Column(TypeName = "decimal(4, 2)")]
    public double HourlyRate { get; set; } = 15.0;

    /// <summary>
    /// Gets or sets the availabilities of the tutor.
    /// </summary>
    public ICollection<Availability>? Availabilities { get; set; }
}
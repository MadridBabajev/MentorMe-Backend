using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Public.DTO.v1.Profiles;

/// <summary>
/// Represents the updated profile data for a user.
/// </summary>
public class UpdatedProfileData
{
    /// <summary>
    /// Gets or sets the updated first name of the user.
    /// </summary>
    [MinLength(2)]
    [MaxLength(32)]
    public string FirstName { get; set; } = default!;

    /// <summary>
    /// Gets or sets the updated last name of the user.
    /// </summary>
    [MinLength(2)]
    [MaxLength(32)]
    public string LastName { get; set; } = default!;

    /// <summary>
    /// Gets or sets the updated mobile phone number of the user.
    /// </summary>
    [MinLength(8)]
    [MaxLength(14)]
    public string MobilePhone { get; set; } = default!;

    /// <summary>
    /// Gets or sets the updated title of the user.
    /// </summary>
    [MaxLength(128)]
    public string Title { get; set; } = "";

    /// <summary>
    /// Gets or sets the updated biography of the user.
    /// </summary>
    [MaxLength(2048)]
    public string Bio { get; set; } = "";

    /// <summary>
    /// Gets or sets the updated profile picture of the user (if available).
    /// </summary>
    public byte[]? ProfilePicture { get; set; }

    /// <summary>
    /// Gets or sets the updated user type.
    /// </summary>
    public string UserType { get; set; } = default!;

    /// <summary>
    /// Gets or sets the updated hourly rate of the user. (Null for students)
    /// </summary>
    [Column(TypeName = "decimal(4, 2)")]
    public double? HourlyRate { get; set; }
}
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BLL.DTO.Subjects;

namespace Public.DTO.v1.Profiles;

/// <summary>
/// Represents the base profile data for a user.
/// </summary>
public class BaseProfileData
{
    /// <summary>
    /// Gets or sets the unique identifier of the user.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether notifications are enabled for the user.
    /// </summary>
    public bool NotificationsEnabled { get; set; }

    /// <summary>
    /// Gets or sets the first name of the user.
    /// </summary>
    [MinLength(2)]
    [MaxLength(32)]
    public string FirstName { get; set; } = default!;

    /// <summary>
    /// Gets or sets the last name of the user.
    /// </summary>
    [MinLength(2)]
    [MaxLength(32)]
    public string LastName { get; set; } = default!;

    /// <summary>
    /// Gets or sets the mobile phone number of the user.
    /// </summary>
    [MinLength(8)]
    [MaxLength(14)]
    public string MobilePhone { get; set; } = default!;

    /// <summary>
    /// Gets or sets the balance of the user.
    /// </summary>
    [Column(TypeName = "decimal(7, 2)")]
    public double Balance { get; set; } = 0.0;

    /// <summary>
    /// Gets or sets the average rating of the user.
    /// </summary>
    [Column(TypeName = "decimal(2, 1)")]
    public double AverageRating { get; set; } = 1.0;

    /// <summary>
    /// Gets or sets the title of the user.
    /// </summary>
    [MaxLength(64)]
    public string Title { get; set; } = "";

    /// <summary>
    /// Gets or sets the biography of the user.
    /// </summary>
    [MaxLength(2048)]
    public string Bio { get; set; } = "";

    /// <summary>
    /// Gets or sets the profile picture of the user (if available).
    /// </summary>
    public byte[]? ProfilePicture { get; set; }

    /// <summary>
    /// Gets or sets the subjects associated with the user.
    /// </summary>
    public IEnumerable<BLLSubjectListElement>? Subjects { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the user's profile is public.
    /// </summary>
    public bool IsPublic { get; set; } = false;

    /// <summary>
    /// Gets or sets the number of classes the user has.
    /// </summary>
    public int NumberOfClasses { get; set; } = 0;
}
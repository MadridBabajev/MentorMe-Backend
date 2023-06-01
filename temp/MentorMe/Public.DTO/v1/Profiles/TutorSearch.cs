namespace Public.DTO.v1.Profiles;

/// <summary>
/// Represents the search result data for a tutor.
/// </summary>
public class TutorSearch
{
    /// <summary>
    /// Gets or sets the unique identifier of the tutor.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the first name of the tutor.
    /// </summary>
    public string FirstName { get; set; } = default!;

    /// <summary>
    /// Gets or sets the last name of the tutor.
    /// </summary>
    public string LastName { get; set; } = default!;

    /// <summary>
    /// Gets or sets the title of the tutor.
    /// </summary>
    public string Title { get; set; } = "";

    /// <summary>
    /// Gets or sets the hourly rate of the tutor.
    /// </summary>
    public double HourlyRate { get; set; }

    /// <summary>
    /// Gets or sets the average rating of the tutor.
    /// </summary>
    public double AverageRating { get; set; }

    /// <summary>
    /// Gets or sets the number of classes tutored by the tutor.
    /// </summary>
    public int ClassesTutored { get; set; }

    /// <summary>
    /// Gets or sets the profile picture of the tutor (if available).
    /// </summary>
    public byte[]? ProfilePicture { get; set; }
}
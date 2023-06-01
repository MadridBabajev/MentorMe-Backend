namespace Public.DTO.v1.Profiles;

/// <summary>
/// Represents the filters used for searching tutors.
/// </summary>
public class TutorSearchFilters
{
    /// <summary>
    /// Gets or sets the first name filter for tutors.
    /// </summary>
    public string? FirstName { get; set; }

    /// <summary>
    /// Gets or sets the last name filter for tutors.
    /// </summary>
    public string? LastName { get; set; }

    /// <summary>
    /// Gets or sets the minimum classes count filter for tutors.
    /// </summary>
    public int? MinClassesCount { get; set; }

    /// <summary>
    /// Gets or sets the maximum classes count filter for tutors.
    /// </summary>
    public int? MaxClassesCount { get; set; }

    /// <summary>
    /// Gets or sets the minimum hourly rate filter for tutors.
    /// </summary>
    public double? MinHourlyRate { get; set; }

    /// <summary>
    /// Gets or sets the maximum hourly rate filter for tutors.
    /// </summary>
    public double? MaxHourlyRate { get; set; }

    /// <summary>
    /// Gets or sets the minimum average rating filter for tutors.
    /// </summary>
    public double? MinAverageRating { get; set; }

    /// <summary>
    /// Gets or sets the maximum average rating filter for tutors.
    /// </summary>
    public double? MaxAverageRating { get; set; }

    /// <summary>
    /// Gets or sets the list of subject IDs for filtering tutors by subjects.
    /// </summary>
    public List<Guid>? SubjectIds { get; set; }
}
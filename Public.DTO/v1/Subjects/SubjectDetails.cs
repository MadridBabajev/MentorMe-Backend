namespace Public.DTO.v1.Subjects;

/// <summary>
/// Represents the details of a subject.
/// </summary>
public class SubjectDetails
{
    /// <summary>
    /// Gets or sets the unique identifier of the subject.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the subject.
    /// </summary>
    public string Name { get; set; } = default!;

    /// <summary>
    /// Gets or sets the description of the subject.
    /// </summary>
    public string Description { get; set; } = default!;

    /// <summary>
    /// Gets or sets the number of tutors who teach the subject.
    /// </summary>
    public int TaughtBy { get; set; }

    /// <summary>
    /// Gets or sets the number of students who have learned the subject.
    /// </summary>
    public int LearnedBy { get; set; }

    /// <summary>
    /// Gets or sets the picture associated with the subject (if available).
    /// </summary>
    public byte[]? SubjectPicture { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the subject is added.
    /// </summary>
    public bool? IsAdded { get; set; }
}
namespace Public.DTO.v1.Subjects;

/// <summary>
/// Represents an element in a subject list.
/// </summary>
public class SubjectListElement
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
    /// Gets or sets the picture associated with the subject (if available).
    /// </summary>
    public byte[]? SubjectPicture { get; set; }
}
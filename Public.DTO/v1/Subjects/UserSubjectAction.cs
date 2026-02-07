namespace Public.DTO.v1.Subjects;

/// <summary>
/// Represents an action performed by a user on a subject.
/// </summary>
public class UserSubjectAction
{
    /// <summary>
    /// Gets or sets the unique identifier of the subject.
    /// </summary>
    public Guid SubjectId { get; set; }

    /// <summary>
    /// Gets or sets the action performed on the subject.
    /// </summary>
    public ESubjectAction SubjectAction { get; set; }
}
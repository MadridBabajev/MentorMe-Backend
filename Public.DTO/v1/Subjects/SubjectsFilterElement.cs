namespace Public.DTO.v1.Subjects;

/// <summary>
/// Represents an element in a subjects filter.
/// </summary>
public class SubjectsFilterElement
{
    /// <summary>
    /// Gets or sets the unique identifier of the subject.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the subject.
    /// </summary>
    public string Name { get; set; } = default!;
}
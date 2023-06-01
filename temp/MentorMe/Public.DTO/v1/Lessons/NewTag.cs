namespace Public.DTO.v1.Lessons;

/// <summary>
/// Represents a new tag to be added to a lesson.
/// </summary>
public class NewTag
{
    /// <summary>
    /// Gets or sets the unique identifier of the lesson.
    /// </summary>
    public Guid LessonId { get; set; }

    /// <summary>
    /// Gets or sets the name of the new tag.
    /// </summary>
    public string Name { get; set; } = default!;

    /// <summary>
    /// Gets or sets the description of the new tag.
    /// </summary>
    public string Description { get; set; } = default!;
}
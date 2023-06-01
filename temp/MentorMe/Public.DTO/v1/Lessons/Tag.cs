namespace Public.DTO.v1.Lessons;

/// <summary>
/// Represents a tag used for categorizing lessons.
/// </summary>
public class Tag
{
    /// <summary>
    /// Gets or sets the unique identifier of the tag.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the tag.
    /// </summary>
    public string Name { get; set; } = default!;

    /// <summary>
    /// Gets or sets the description of the tag.
    /// </summary>
    public string Description { get; set; } = default!;

    /// <summary>
    /// Gets or sets the date and time when the tag was added.
    /// </summary>
    public DateTime AddedAt { get; set; }
}
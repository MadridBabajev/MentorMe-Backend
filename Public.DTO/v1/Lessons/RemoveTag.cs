namespace Public.DTO.v1.Lessons;

/// <summary>
/// A DTO that contains an Id of the tag user wishes to remove
/// </summary>
public class RemoveTag
{
    /// <summary>
    /// A tag id
    /// </summary>
    public Guid TagId { get; set; }
}
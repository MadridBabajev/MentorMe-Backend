namespace Public.DTO.v1.Lessons;

/// <summary>
/// A DTO containing the id of the lesson created
/// </summary>
public class ReserveLessonResponse
{
    /// <summary>
    /// The lesson id
    /// </summary>
    public Guid LessonId { get; set; }
}
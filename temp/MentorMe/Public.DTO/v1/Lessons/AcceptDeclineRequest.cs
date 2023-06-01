namespace Public.DTO.v1.Lessons;

/// <summary>
/// Represents a request to accept or decline a lesson.
/// </summary>
public class AcceptDeclineRequest
{
    /// <summary>
    /// Gets or sets the unique identifier of the lesson.
    /// </summary>
    public Guid LessonId { get; set; }

    /// <summary>
    /// Gets or sets the tutor's decision regarding the lesson.
    /// </summary>
    public ETutorDecision TutorDecision { get; set; }
}
namespace Public.DTO.v1.Lessons;

/// <summary>
/// Represents a request to reserve a lesson.
/// </summary>
public class ReserveLessonRequest
{
    /// <summary>
    /// Gets or sets the unique identifier of the tutor.
    /// </summary>
    public Guid TutorId { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the payment method.
    /// </summary>
    public Guid PaymentMethodId { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the subject.
    /// </summary>
    public Guid SubjectId { get; set; }

    /// <summary>
    /// Gets or sets the start time of the lesson.
    /// </summary>
    public DateTime LessonStartTime { get; set; }

    /// <summary>
    /// Gets or sets the end time of the lesson.
    /// </summary>
    public DateTime LessonEndTime { get; set; }
}
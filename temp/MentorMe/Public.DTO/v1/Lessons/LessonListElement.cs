using Domain.Enums;

namespace Public.DTO.v1.Lessons;

/// <summary>
/// Represents an element in a lesson list.
/// </summary>
public class LessonListElement
{
    /// <summary>
    /// Gets or sets the unique identifier of the lesson.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the start time of the lesson.
    /// </summary>
    public DateTime StartTime { get; set; }

    /// <summary>
    /// Gets or sets the end time of the lesson.
    /// </summary>
    public DateTime EndTime { get; set; }

    /// <summary>
    /// Gets or sets the state of the lesson.
    /// </summary>
    public ELessonState LessonState { get; set; }

    /// <summary>
    /// Gets or sets the full name of the tutor for the lesson.
    /// </summary>
    public string TutorFullName { get; set; } = default!;

    /// <summary>
    /// Gets or sets the full name of the student for the lesson.
    /// </summary>
    public string StudentFullName { get; set; } = default!;

    /// <summary>
    /// Gets or sets the name of the subject for the lesson.
    /// </summary>
    public string SubjectName { get; set; } = default!;

    /// <summary>
    /// Gets or sets the price of the lesson.
    /// </summary>
    public double LessonPrice { get; set; }
}
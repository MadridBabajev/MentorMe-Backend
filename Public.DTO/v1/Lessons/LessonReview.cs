using Domain.Enums;

namespace Public.DTO.v1.Lessons;

/// <summary>
/// Represents a user review for a lesson.
/// </summary>
public class UserReview
{
    /// <summary>
    /// Gets or sets the unique identifier of the lesson.
    /// </summary>
    public Guid LessonId { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the tutor.
    /// </summary>
    public Guid TutorId { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the student.
    /// </summary>
    public Guid StudentId { get; set; }

    /// <summary>
    /// Gets or sets the type of the review.
    /// </summary>
    public EReviewType ReviewType { get; set; }

    /// <summary>
    /// Gets or sets the rating given in the review.
    /// </summary>
    public int Rating { get; set; }

    /// <summary>
    /// Gets or sets the comment provided in the review.
    /// </summary>
    public string Comment { get; set; } = default!;
}
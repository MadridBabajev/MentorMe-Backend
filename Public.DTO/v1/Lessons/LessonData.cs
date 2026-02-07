using Domain.Enums;
using Public.DTO.v1.Profiles;
using Public.DTO.v1.Profiles.Secondary;
using Public.DTO.v1.Subjects;

namespace Public.DTO.v1.Lessons;

/// <summary>
/// Represents the data of a lesson.
/// </summary>
public class LessonData
{
    /// <summary>
    /// Gets or sets the unique identifier of the lesson.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the payment associated with the lesson.
    /// </summary>
    public Guid PaymentId { get; set; }

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
    /// Gets or sets the price of the lesson.
    /// </summary>
    public double Price { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the lesson has been viewed by the tutor.
    /// </summary>
    public bool ViewedByTutor { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the user can write a review for the lesson.
    /// </summary>
    public bool UserCanWriteReview { get; set; }

    /// <summary>
    /// Gets or sets the payment method used by the student for the lesson.
    /// </summary>
    public StudentPaymentMethod StudentPaymentMethod { get; set; } = default!;

    /// <summary>
    /// Gets or sets the subject of the lesson.
    /// </summary>
    public SubjectListElement Subject { get; set; } = default!;

    /// <summary>
    /// Gets or sets the profile card data of the student for the lesson.
    /// </summary>
    public ProfileCardData LessonStudent { get; set; } = default!;

    /// <summary>
    /// Gets or sets the profile card data of the tutor for the lesson.
    /// </summary>
    public ProfileCardData LessonTutor { get; set; } = default!;

    /// <summary>
    /// Gets or sets the tags associated with the lesson.
    /// </summary>
    public ICollection<Tag> Tags { get; set; } = default!;
}
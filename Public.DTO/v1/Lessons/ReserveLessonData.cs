using Public.DTO.v1.Profiles.Secondary;
using Public.DTO.v1.Subjects;

namespace Public.DTO.v1.Lessons;

/// <summary>
/// Represents data required for reserving a lesson.
/// </summary>
public class ReserveLessonData
{
    /// <summary>
    /// Gets or sets the list of payment methods available for the student.
    /// </summary>
    public List<StudentPaymentMethod>? PaymentMethods { get; set; }

    /// <summary>
    /// Gets or sets the list of availabilities for scheduling the lesson.
    /// </summary>
    public List<Availability>? Availabilities { get; set; }

    /// <summary>
    /// Gets or sets the list of subjects available for the lesson.
    /// </summary>
    public List<SubjectsFilterElement>? Subjects { get; set; }
}
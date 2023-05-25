using Domain.Enums;
using Public.DTO.v1.Profiles;
using Public.DTO.v1.Subjects;

namespace Public.DTO.v1.Lessons;

public class LessonData
{
    public Guid Id { get; set; }
    public Guid PaymentId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public ELessonState LessonState { get; set; }
    public double Price { get; set; }
    public bool ViewedByTutor { get; set; }
    public bool UserCanWriteReview { get; set; }
    public StudentPaymentMethod StudentPaymentMethod { get; set; }
    public SubjectListElement Subject { get; set; }
    public ProfileCardData LessonStudent { get; set; }
    public ProfileCardData LessonTutor { get; set; }
    public ICollection<Tag> Tags { get; set; }
}

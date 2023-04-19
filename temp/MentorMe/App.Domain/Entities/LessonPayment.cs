using Base.DomainEntity;

namespace Domain.Entities;

public class LessonPayment: BaseDomainEntity
{
    public Guid PaymentId { get; set; }
    public Payment? Payment { get; set; }
    public Guid LessonId { get; set; }
    public Lesson Lesson { get; set; } = default!;
    public Guid StudentId { get; set; }
    public Student Student { get; set; } = default!;
    public Guid TutorId { get; set; }
    public Tutor Tutor { get; set; } = default!;
}

using Base.DomainEntity;

namespace Domain.Entities;

public class LessonParticipation: BaseDomainEntity
{
    public Guid StudentId { get; set; }
    public Student? Student { get; set; }
    public Guid LessonId { get; set; }
    public Lesson? Lesson { get; set; }
}

using Base.DomainEntity;

namespace Domain.Entities;

public class TutorSubject: BaseDomainEntity
{
    public Guid TutorId { get; set; }
    public Tutor? Tutor { get; set; }
    public Guid SubjectId { get; set; }
    public Subject? Subject { get; set; }
}

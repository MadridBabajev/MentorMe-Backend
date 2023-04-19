using Base.DomainEntity;

namespace Domain.Entities;

public class StudentSubject: BaseDomainEntity
{
    public Guid StudentId { get; set; }
    public Student Student { get; set; } = default!;
    public Guid SubjectId { get; set; }
    public Subject Subject { get; set; } = default!;
}

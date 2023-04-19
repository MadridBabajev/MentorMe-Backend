using System.ComponentModel.DataAnnotations;
using Base.DomainEntity;

namespace Domain.Entities;

public class Tag: BaseDomainEntity
{
    [MinLength(2)]
    [MaxLength(32)]
    public string Name { get; set; } = default!;
    [MinLength(2)]
    [MaxLength(500)]
    public string Description { get; set; } = default!;
    
    // Nav
    public Guid TutorId { get; set; }
    public Tutor Tutor { get; set; } = default!;
    public Guid LessonId { get; set; }
    public Lesson Lesson { get; set; } = default!;
}

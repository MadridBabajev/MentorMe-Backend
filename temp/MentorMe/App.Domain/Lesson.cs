using System.ComponentModel.DataAnnotations;
using Base.DomainEntity;
using Domain.Enums;

namespace Domain;

public class Lesson: BaseDomainEntity
{
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    [Range(typeof(decimal), "30", "181")]
    public int LessonDuration { get; set; }
    [Range(typeof(decimal), "1", "11")] 
    public int ParticipantCount { get; set; } = 1;
    public bool IsCanceled { get; set; }
    public ELessonType LessonType { get; set; }
    public ELessonState LessonState { get; set; }
    
    // Nav
    public Guid TutorId { get; set; }
    public Tutor Tutor { get; set; } = default!;
    public Guid SubjectId { get; set; }
    public Subject Subject { get; set; } = default!;
    public ICollection<LessonParticipation>? LessonParticipations { get; set; }
    public ICollection<Tag>? Tags { get; set; }
    public ICollection<Notification>? Notifications { get; set; }
    public ICollection<LessonPayment>? Payments { get; set; }
    public Cancellation? Cancellation { get; set; }
}

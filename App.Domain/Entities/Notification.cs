using System.ComponentModel.DataAnnotations;
using Base.DomainEntity;
using Domain.Enums;
using Domain.Identity;

namespace Domain.Entities;

public class Notification: BaseDomainEntity
{
    [MinLength(2)]
    [MaxLength(500)]
    [Required]
    public string Text { get; set; } = default!;
    public DateTime Time { get; set; }
    public bool WasSeen { get; set; }
    public ENotificationType NotificationType { get; set; }
    
    // Nav
    public Guid AppUserId { get; set; }
    public AppUser? AppUser { get; set; } = default!;
    public Guid? LessonPaymentId { get; set; }
    public LessonPayment? LessonPayment { get; set; }
    public Guid? LessonId { get; set; }
    public Lesson? Lesson { get; set; }
    public Guid? MessageId { get; set; }
    public Message? Message { get; set; }
}

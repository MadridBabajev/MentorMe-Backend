using System.ComponentModel.DataAnnotations;
using Base.DomainEntity;
using Domain.Enums;

namespace Domain;

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
    public Guid UserId { get; set; }
    public DialogFeatureUser DialogFeatureUser { get; set; } = default!;
    public Guid? LessonPaymentId { get; set; }
    public LessonPayment? LessonPayment { get; set; }
    public Guid? LessonId { get; set; }
    public Lesson? Lesson { get; set; }
    public Guid? MessageId { get; set; }
    public Message? Message { get; set; }
}

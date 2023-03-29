using System.ComponentModel.DataAnnotations;
using Base.DomainEntity;

namespace Domain;

public class Message: BaseDomainEntity
{
    [MaxLength(500)]
    [Required]
    public string Content { get; set; } = default!;
    public DateTime Timestamp { get; set; } = DateTime.Now;
    
    // Nav
    // Making SenderId a nullable allows sending system-generated messages
    public Guid? SenderId { get; set; }
    public DialogFeatureUser? DialogFeatureUser { get; set; }
    public Guid DialogId { get; set; }
    public Dialog Dialog { get; set; } = default!;
}

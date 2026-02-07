using System.ComponentModel.DataAnnotations;
using Base.DomainEntity;
using Domain.Identity;

namespace Domain.Entities;

public class Message: BaseDomainEntity
{
    [MaxLength(500)]
    [Required]
    public string Content { get; set; } = default!;
    public DateTime Timestamp { get; set; } = DateTime.Now;
    
    // Nav
    // Making SenderId a nullable allows sending system-generated messages
    public Guid? SenderId { get; set; }
    public AppUser? AppUser { get; set; }
    public Guid DialogId { get; set; }
    public Dialog Dialog { get; set; } = default!;
}

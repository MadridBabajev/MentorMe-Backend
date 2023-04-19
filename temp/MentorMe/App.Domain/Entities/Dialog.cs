using System.ComponentModel.DataAnnotations;
using Base.DomainEntity;

namespace Domain.Entities;

public class Dialog: BaseDomainEntity
{
    [MinLength(2)]
    [MaxLength(64)]
    public string? Name { get; set; }
    public DateTime LastUpdated { get; set; }

    // Nav
    public ICollection<DialogParticipant> Participants { get; set; } = default!;
    public ICollection<Message>? Messages { get; set; }
}
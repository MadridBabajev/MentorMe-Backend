using Base.DomainEntity;
using Domain.Enums;
using Domain.Identity;

namespace Domain.Entities;

public class DialogParticipant: BaseDomainEntity
{
    public EParticipantType ParticipantType { get; set; }
    
    // Nav
    public Guid AppUserId { get; set; }
    public AppUser? AppUser { get; set; }
    public Guid DialogId { get; set; }
    public Dialog? Dialog { get; set; } = default!;
}

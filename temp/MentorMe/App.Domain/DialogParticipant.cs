using Base.DomainEntity;
using Domain.Enums;

namespace Domain;

public class DialogParticipant: BaseDomainEntity
{
    public EParticipantType ParticipantType { get; set; }
    
    // Nav
    public Guid DialogFeatureUserId { get; set; }
    public DialogFeatureUser? DialogFeatureUser { get; set; }
    public Guid DialogId { get; set; }
    public Dialog? Dialog { get; set; } = default!;
}

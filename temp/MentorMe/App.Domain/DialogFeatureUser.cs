using Base.DomainEntity;

namespace Domain;

public class DialogFeatureUser: BaseDomainEntity
{
    // Nav
    public ICollection<Notification>? Notifications { get; set; }
    public ICollection<DialogParticipant>? Participants { get; set; }
    public ICollection<Message>? Messages { get; set; }
    public Tutor? TutorProfile { get; set; }
    public Student? StudentProfile { get; set; }
}

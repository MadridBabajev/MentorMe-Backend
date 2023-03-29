namespace Domain;

public class TutorSubject
{
    public Guid Id { get; set; }
    public Guid TutorId { get; set; }
    public Tutor Tutor { get; set; } = default!;
    public Guid SubjectId { get; set; }
    public Subject Subject { get; set; } = default!;
}

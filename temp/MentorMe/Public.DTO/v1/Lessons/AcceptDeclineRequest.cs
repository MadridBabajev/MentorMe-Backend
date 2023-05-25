namespace Public.DTO.v1.Lessons;

public class AcceptDeclineRequest
{
    public Guid LessonId { get; set; }
    public ETutorDecision TutorDecision { get; set; }
}
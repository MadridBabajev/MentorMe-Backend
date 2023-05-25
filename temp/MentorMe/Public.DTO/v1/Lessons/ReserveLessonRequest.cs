namespace Public.DTO.v1.Lessons;

public class ReserveLessonRequest
{
    public Guid TutorId { get; set; }
    public Guid PaymentMethodId { get; set; }
    public Guid SubjectId { get; set; }
    public DateTime LessonStartTime { get; set; }
    public DateTime LessonEndTime { get; set; }
}
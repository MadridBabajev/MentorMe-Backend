using Domain.Enums;

namespace Public.DTO.v1.Lessons;

public class UserReview
{
    public Guid LessonId { get; set; }
    public Guid TutorId { get; set; }
    public Guid StudentId { get; set; }
    public EReviewType ReviewType { get; set; }
    public int Rating { get; set; }
    public string Comment { get; set; }
}
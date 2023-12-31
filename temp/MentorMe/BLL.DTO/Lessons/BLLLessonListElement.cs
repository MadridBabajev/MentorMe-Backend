using Base.DAL.Contracts;
using Domain.Enums;

namespace BLL.DTO.Lessons;

public class BLLLessonListElement: IDomainEntityId
{
    public Guid Id { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public ELessonState LessonState { get; set; }
    public string TutorFullName { get; set; } = default!;
    public string StudentFullName { get; set; } = default!;
    public string SubjectName { get; set; } = default!;
    
    public double LessonPrice { get; set; }
}
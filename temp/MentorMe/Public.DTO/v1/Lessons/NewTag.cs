namespace Public.DTO.v1.Lessons;

public class NewTag
{
    public Guid LessonId { get; set; }
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
}
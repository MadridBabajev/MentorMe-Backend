namespace Public.DTO.v1.Subjects;

public class SubjectDetails
{
    public Guid Id { get; set; }

    public string Name { get; set; } = default!;
    
    public string Description { get; set; } = default!;
    
    public int TaughtBy { get; set; }
    
    public int LearnedBy { get; set; }
    
    public byte[]? SubjectPicture { get; set; } = default!;
}
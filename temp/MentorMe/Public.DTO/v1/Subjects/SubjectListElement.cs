namespace Public.DTO.v1.Subjects;

public class SubjectListElement
{
    public Guid Id { get; set; }

    public string Name { get; set; } = default!;
    
    public byte[]? SubjectPicture { get; set; }
}
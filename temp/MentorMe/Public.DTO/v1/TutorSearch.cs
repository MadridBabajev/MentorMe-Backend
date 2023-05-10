namespace Public.DTO.v1;

public class TutorSearch
{
    public Guid Id { get; set; }

    public string FirstName { get; set; } = default!;

    public string LastName { get; set; } = default!;
    
    public string Title { get; set; } = "";
    
    public byte[]? ProfilePicture { get; set; }
}
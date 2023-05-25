namespace Public.DTO.v1.Profiles;

public class ProfileCardData
{
    public Guid Id { get; set; }
    public string FullName { get; set; }
    public byte[]? ProfilePicture { get; set; }
    public double AverageRating { get; set; }
}
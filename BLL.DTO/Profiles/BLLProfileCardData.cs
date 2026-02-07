using Base.DAL.Contracts;

namespace BLL.DTO.Profiles;

public class BLLProfileCardData: IDomainEntityId
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = default!;
    public byte[]? ProfilePicture { get; set; }
    public double AverageRating { get; set; }
}
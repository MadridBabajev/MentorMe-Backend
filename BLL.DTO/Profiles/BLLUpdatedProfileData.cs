using Base.DAL.Contracts;

namespace BLL.DTO.Profiles;

public class BLLUpdatedProfileData: IDomainEntityId
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string MobilePhone { get; set; } = default!;
    public string Title { get; set; } = "";
    public string Bio { get; set; } = "";
    public byte[]? ProfilePicture { get; set; }
    public string UserType { get; set; }
    public double? HourlyRate { get; set; } // is a null for students
}
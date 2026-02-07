using Base.DAL.Contracts;

namespace BLL.DTO.Profiles;

// ReSharper disable once InconsistentNaming
public class BLLTutorSearch : IDomainEntityId
{
    public Guid Id { get; set; }

    public string FirstName { get; set; } = default!;

    public string LastName { get; set; } = default!;

    public string Title { get; set; } = "";
    
    public double HourlyRate { get; set; }
    
    public double AverageRating { get; set; }
    
    public int ClassesTutored { get; set; }

    public byte[]? ProfilePicture { get; set; }
}

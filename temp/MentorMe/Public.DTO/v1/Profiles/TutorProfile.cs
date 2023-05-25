using System.ComponentModel.DataAnnotations.Schema;

namespace Public.DTO.v1.Profiles;

public class TutorProfile: BaseProfileData
{
    [Column(TypeName = "decimal(4, 2)")] 
    public double HourlyRate { get; set; } = 15.0;
    public ICollection<TutorAvailability>? Availabilities { get; set; }
}
using System.ComponentModel.DataAnnotations.Schema;
using Public.DTO.v1.Profiles.Secondary;

namespace Public.DTO.v1.Profiles;

public class TutorProfile: BaseProfileData
{
    [Column(TypeName = "decimal(4, 2)")] 
    public double HourlyRate { get; set; } = 15.0;
    public ICollection<Availability>? Availabilities { get; set; }
}
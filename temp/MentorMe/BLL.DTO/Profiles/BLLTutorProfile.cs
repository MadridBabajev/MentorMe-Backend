using System.ComponentModel.DataAnnotations.Schema;

namespace BLL.DTO.Profiles;

public class BLLTutorProfile: BLLBaseProfile
{
    [Column(TypeName = "decimal(4, 2)")] 
    public double HourlyRate { get; set; } = 15.0;
    public ICollection<BLLAvailability>? Availabilities { get; set; }
}
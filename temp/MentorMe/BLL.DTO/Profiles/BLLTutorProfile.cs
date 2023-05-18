using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities;

namespace BLL.DTO.Profiles;

public class BLLTutorProfile: BLLBaseProfile
{
    [Column(TypeName = "decimal(4, 2)")] 
    public double HourlyRate { get; set; } = 15.0;
    public ICollection<BLLTutorAvailability>? Availabilities { get; set; }
}
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities;

namespace Public.DTO.v1;

public class TutorProfile: UserBase
{
    [Column(TypeName = "decimal(4, 2)")] 
    public double HourlyRate { get; set; } = 15.0;
    public ICollection<TutorAvailability>? Availabilities { get; set; }
}
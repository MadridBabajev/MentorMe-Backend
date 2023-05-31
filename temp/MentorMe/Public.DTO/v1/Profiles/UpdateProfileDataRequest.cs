using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Public.DTO.v1.Profiles;

public class UpdateProfileDataRequest
{
    [MinLength(2)]
    [MaxLength(32)]
    public string FirstName { get; set; } = default!;
    [MinLength(2)]
    [MaxLength(32)]
    public string LastName { get; set; } = default!;
    [MinLength(8)]
    [MaxLength(14)]
    public string MobilePhone { get; set; } = default!;
    [MaxLength(128)]
    public string Title { get; set; } = "";
    
    [MaxLength(2048)]
    public string Bio { get; set; } = "";

    public string? ProfilePicture { get; set; } // Base64 string
    public string UserType { get; set; }
    
    [Column(TypeName = "decimal(4, 2)")] 
    public double? HourlyRate { get; set; } // is a null for students
}
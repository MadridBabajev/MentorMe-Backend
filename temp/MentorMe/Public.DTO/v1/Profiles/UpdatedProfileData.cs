using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Enums;

namespace Public.DTO.v1.Profiles;

public class UpdatedProfileData
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

    public byte[]? ProfilePicture { get; set; }
    public string UserType { get; set; }
    
    [Column(TypeName = "decimal(4, 2)")] 
    public double? HourlyRate { get; set; } // is a null for students
}
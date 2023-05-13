using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BLL.DTO;

namespace Public.DTO.v1;

public class UserBase
{
    public Guid Id { get; set; }
    public bool NotificationsEnabled { get; set; }
    [MinLength(2)]
    [MaxLength(32)]
    public string FirstName { get; set; } = default!;
    [MinLength(2)]
    [MaxLength(32)]
    public string LastName { get; set; } = default!;
    [MinLength(8)]
    [MaxLength(14)]
    public string MobilePhone { get; set; } = default!;
    // public EUserType AppUserType { get; set; } = default!;
    [Column(TypeName = "decimal(7, 2)")] 
    public double Balance { get; set; } = 0.0;
    [Column(TypeName = "decimal(2, 1)")] 
    public double AverageRating { get; set; } = 1.0;
    [MaxLength(64)]
    public string Title { get; set; } = "";
    
    [MaxLength(2048)]
    public string Bio { get; set; } = "";

    public byte[]? ProfilePicture { get; set; }
    public IEnumerable<BLLSubjectListElement> Subjects { get; set; }
    
    public bool IsPublic { get; set; }
}
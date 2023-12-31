using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Base.DAL.Contracts;
using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace Domain.Identity;

public class AppUser : IdentityUser<Guid>, IDomainEntityId
{
    public bool NotificationsEnabled { get; set; }
    public bool IsBlocked { get; set; }
    [MinLength(1)]
    [MaxLength(32)]
    public string FirstName { get; set; } = default!;
    [MinLength(1)]
    [MaxLength(32)]
    public string LastName { get; set; } = default!;
    [MinLength(8)]
    [MaxLength(14)]
    [Required]
    public string MobilePhone { get; set; } = default!;
    public EUserType AppUserType { get; set; } = EUserType.Student;
    [Column(TypeName = "decimal(7, 2)")] 
    public double Balance { get; set; }

    public ECountry Country { get; set; } = ECountry.Estonia;
    
    [MaxLength(64)]
    public string Title { get; set; } = "";
    
    [MaxLength(2048)]
    public string Bio { get; set; } = "";

    public byte[]? ProfilePicture { get; set; }
    
    // nav
    public Tutor? Tutor { get; set; }
    public Student? Student { get; set; }
    public ICollection<AppRefreshToken>? AppRefreshTokens { get; set; }
    public ICollection<Notification>? Notifications { get; set; }
    public ICollection<DialogParticipant>? Participations { get; set; }
    public ICollection<Message>? Messages { get; set; }
}

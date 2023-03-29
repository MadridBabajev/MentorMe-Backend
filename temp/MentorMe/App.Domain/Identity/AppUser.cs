using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Base.DAL.Contracts;
using Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace Domain.Identity;

public class AppUser : IdentityUser<Guid>, IDomainEntityId
{
    public bool NotificationsEnabled { get; set; } = false;
    public bool IsBlocked { get; set; }
    [MinLength(2)]
    [MaxLength(32)]
    public string FirstName { get; set; } = default!;
    [MinLength(2)]
    [MaxLength(32)]
    public string LastName { get; set; } = default!;
    [MinLength(8)]
    [MaxLength(14)]
    [Required]
    public string MobilePhone { get; set; } = default!;
    public EUserType AppUserType { get; set; } = EUserType.Student;
    [Column(TypeName = "decimal(7, 2)")] 
    public double Balance { get; set; } = 0.0;
}

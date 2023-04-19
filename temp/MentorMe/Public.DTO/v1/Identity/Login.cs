using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace Public.DTO.v1.Identity;

public class Login
{
    [StringLength(maximumLength:128, MinimumLength = 5, ErrorMessage = "Wrong length on email")] 
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;
    public EUserType UserType { get; set; } = default!;
}

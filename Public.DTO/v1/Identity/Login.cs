using System.ComponentModel.DataAnnotations;

namespace Public.DTO.v1.Identity;

/// <summary>
/// Represents a login DTO.
/// </summary>
public class Login
{
    /// <summary>
    /// An email address the user is trying to log in with.
    /// </summary>
    /// <remarks>
    /// The email address should be between 5 and 128 characters long.
    /// </remarks>
    [StringLength(maximumLength:128, MinimumLength = 5, ErrorMessage = "Wrong length on email")] 
    public string Email { get; set; } = default!;
    
    /// <summary>
    /// A password the user is trying to log in with..
    /// </summary>
    public string Password { get; set; } = default!;
    
    /// <summary>
    /// A value indicating whether the user is a tutor.
    /// </summary>
    public bool IsTutor { get; set; } = default!;
}

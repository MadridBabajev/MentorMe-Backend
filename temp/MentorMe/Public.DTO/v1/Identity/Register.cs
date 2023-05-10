using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace Public.DTO.v1.Identity;

/// <summary>
/// 
/// </summary>
public class Register
{
    // TODO Implement a better fields checking
    /// <summary>
    /// An email address the user is trying to register with.
    /// </summary>
    /// <remarks>
    /// The email address should be between 5 and 128 characters long.
    /// </remarks>
    [StringLength(128, MinimumLength = 5, ErrorMessage = "Incorrect length")]
    public string Email { get; set; } = default!;
    
    /// <summary>
    /// A mobile phone the user is trying to register with.
    /// </summary>
    /// <remarks>
    /// The phone number should be between 1 and 128 characters long.
    /// </remarks>
    [StringLength(128, MinimumLength = 1, ErrorMessage = "Incorrect length")]
    public string MobilePhone { get; set; } = default!;
    
    /// <summary>
    /// A password the user is trying to register with.
    /// </summary>
    /// <remarks>
    /// The password should be between 8 and 128 characters long.
    /// </remarks>
    [StringLength(128, MinimumLength = 8, ErrorMessage = "Incorrect length")]
    public string Password { get; set; } = default!;
    
    /// <summary>
    /// The first name the user is trying to register with.
    /// </summary>
    /// <remarks>
    /// The first name should be between 1 and 128 characters long.
    /// </remarks>
    [StringLength(128, MinimumLength = 1, ErrorMessage = "Incorrect length")]
    public string Firstname { get; set; } = default!;

    /// <summary>
    /// The last name the user is trying to register with.
    /// </summary>
    /// <remarks>
    /// The last name should be between 1 and 128 characters long.
    /// </remarks>
    [StringLength(128, MinimumLength = 1, ErrorMessage = "Incorrect length")]
    public string Lastname { get; set; } = default!;

    /// <summary>
    /// The field that indicates whether the user is a tutor.
    /// </summary>
    public bool IsTutor { get; set; } = default!;
    
    /// <summary>
    /// The country the user marked when registering 
    /// </summary>
    public ECountry Country { get; set; } = default!;
}

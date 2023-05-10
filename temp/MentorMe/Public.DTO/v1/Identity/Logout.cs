namespace Public.DTO.v1.Identity;

/// <summary>
/// Represents a logout DTO.
/// </summary>
public class Logout
{
    /// <summary>
    /// Gets or sets the refresh token.
    /// </summary>
    public string RefreshToken { get; set; } = default!;
}
namespace Public.DTO.v1.Identity;

// ReSharper disable InconsistentNaming
/// <summary>
/// Represents the response DTO to a JWT authentication request.
/// </summary>
public class JWTResponse
{
    /// <summary>
    /// The JWT token used to recognise the user once they sign in to their profile
    /// </summary>
    public string JWT { get; set; } = default!;
    /// <summary>
    /// Refresh token for creating a new JWT once the old one gets expired
    /// Smoothens the user experience
    /// </summary>
    public string RefreshToken { get; set; } = default!;
    
    public int ExpiresIn { get; set; }
}
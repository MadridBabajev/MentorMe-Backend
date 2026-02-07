namespace Public.DTO.v1.Identity;

/// <summary>
/// Represents a model for refreshing access tokens.
/// </summary>
public class RefreshTokenModel
{
    /// <summary>
    /// The nearly expired jwt token 
    /// </summary>
    public string Jwt { get; set; } = default!;
    
    /// <summary>
    /// The refresh token required for creation of a new JWT
    /// </summary>
    public string RefreshToken { get; set; } = default!;
}
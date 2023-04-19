namespace Public.DTO.v1.Identity;

// ReSharper disable InconsistentNaming
public class JWTResponse
{
    public string JWT { get; set; } = default!;
    public string RefreshToken { get; set; } = default!;
}
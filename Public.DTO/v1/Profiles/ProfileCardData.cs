namespace Public.DTO.v1.Profiles;

/// <summary>
/// Represents the profile card data for a user.
/// </summary>
public class ProfileCardData
{
    /// <summary>
    /// Gets or sets the unique identifier of the user.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the full name of the user.
    /// </summary>
    public string FullName { get; set; } = default!;

    /// <summary>
    /// Gets or sets the profile picture of the user (if available).
    /// </summary>
    public byte[]? ProfilePicture { get; set; }

    /// <summary>
    /// Gets or sets the average rating of the user.
    /// </summary>
    public double AverageRating { get; set; }
}
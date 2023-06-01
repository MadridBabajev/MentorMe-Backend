using System.ComponentModel.DataAnnotations;

namespace Public.DTO.v1.Profiles.Secondary;

/// <summary>
/// Represents a new payment method to be added.
/// </summary>
public class NewPaymentMethod
{
    /// <summary>
    /// Gets or sets the details of the new payment method.
    /// </summary>
    [MinLength(2)]
    [MaxLength(128)]
    public string Details { get; set; } = default!;

    /// <summary>
    /// Gets or sets the CVV of the card associated with the new payment method.
    /// </summary>
    [MinLength(1)]
    [MaxLength(32)]
    public string CardCvv { get; set; } = default!;

    /// <summary>
    /// Gets or sets the expiration date of the card associated with the new payment method.
    /// </summary>
    [MinLength(1)]
    [MaxLength(32)]
    public string CardExpirationDate { get; set; } = default!;

    /// <summary>
    /// Gets or sets the card number of the new payment method.
    /// </summary>
    [MinLength(1)]
    [MaxLength(32)]
    public string CardNumber { get; set; } = default!;
}
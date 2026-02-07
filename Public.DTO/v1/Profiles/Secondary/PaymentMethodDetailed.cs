using Domain.Enums;

namespace Public.DTO.v1.Profiles.Secondary;

/// <summary>
/// Represents a detailed payment method.
/// </summary>
public class PaymentMethodDetailed
{
    /// <summary>
    /// Gets or sets the unique identifier of the payment method.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the type of the payment method.
    /// </summary>
    public EPaymentMethod PaymentMethodType { get; set; }

    /// <summary>
    /// Gets or sets the details of the payment method.
    /// </summary>
    public string Details { get; set; } = default!;

    /// <summary>
    /// Gets or sets the CVV of the card associated with the payment method. Default value is "N/A".
    /// </summary>
    public string CardCvv { get; set; } = "N/A";

    /// <summary>
    /// Gets or sets the expiration date of the card associated with the payment method. Default value is "N/A".
    /// </summary>
    public string CardExpirationDate { get; set; } = "N/A";

    /// <summary>
    /// Gets or sets the card number of the payment method. Default value is "N/A".
    /// </summary>
    public string CardNumber { get; set; } = "N/A";

    /// <summary>
    /// Gets or sets the full name of the owner of the payment method. Default value is "N/A".
    /// </summary>
    public string OwnerFullName { get; set; } = "N/A";

    /// <summary>
    /// Gets or sets the country of the owner of the payment method.
    /// </summary>
    public ECountry OwnerCountry { get; set; }
}
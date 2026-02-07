namespace Public.DTO.v1.Profiles.Secondary;

/// <summary>
/// Represents a request to remove a payment method.
/// </summary>
public class RemovePaymentMethod
{
    /// <summary>
    /// Gets or sets the unique identifier of the payment method to be removed.
    /// </summary>
    public Guid PaymentMethodId { get; set; }
}
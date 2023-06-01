using Domain.Enums;

namespace Public.DTO.v1.Profiles.Secondary;

/// <summary>
/// Represents a payment method associated with a student.
/// </summary>
public class StudentPaymentMethod
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
    /// Gets or sets the details of the payment method (optional).
    /// </summary>
    public string? Details { get; set; }
}
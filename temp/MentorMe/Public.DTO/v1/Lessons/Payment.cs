using Domain.Enums;

namespace Public.DTO.v1.Lessons;

/// <summary>
/// Represents a payment made for a lesson.
/// </summary>
public class Payment
{
    /// <summary>
    /// Gets or sets the unique identifier of the payment.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the date of the payment.
    /// </summary>
    public DateTime Date { get; set; }

    /// <summary>
    /// Gets or sets the amount of the payment.
    /// </summary>
    public double Amount { get; set; }

    /// <summary>
    /// Gets or sets any additional fees associated with the payment.
    /// </summary>
    public double? AdditionalFees { get; set; }

    /// <summary>
    /// Gets or sets the description of the payment.
    /// </summary>
    public string Description { get; set; } = default!;

    /// <summary>
    /// Gets or sets the full name of the student who sent the payment.
    /// </summary>
    public string SenderStudentFullName { get; set; } = default!;

    /// <summary>
    /// Gets or sets the full name of the tutor who received the payment.
    /// </summary>
    public string RecipientTutorFullName { get; set; } = default!;

    /// <summary>
    /// Gets or sets the status of the payment.
    /// </summary>
    public EPaymentStatus PaymentStatus { get; set; }

    /// <summary>
    /// Gets or sets the type of payment method used for the payment.
    /// </summary>
    public EPaymentMethod PaymentMethodType { get; set; }
}
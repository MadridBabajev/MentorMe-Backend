using Domain.Enums;

namespace Public.DTO.v1.Lessons;

public class Payment
{
    public Guid Id { get; set; }
    public DateTime Date { get; set; }
    public double Amount { get; set; }
    public double? AdditionalFees { get; set; }
    public string Description { get; set; } = default!;
    public string SenderStudentFullName { get; set; } = default!;
    public string RecipientTutorFullName { get; set; } = default!;
    public EPaymentStatus PaymentStatus { get; set; }
    public EPaymentMethod PaymentMethodType { get; set; }
}
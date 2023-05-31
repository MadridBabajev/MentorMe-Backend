using Domain.Enums;

namespace Public.DTO.v1.Profiles.Secondary;

public class StudentPaymentMethod
{
    public Guid Id { get; set; }
    public EPaymentMethod PaymentMethodType { get; set; }
    public string? Details { get; set; }
}
using Domain.Enums;

namespace Public.DTO.v1.Profiles.Secondary;

public class PaymentMethodDetailed
{
    public Guid Id { get; set; }
    public EPaymentMethod PaymentMethodType { get; set; }
    public string Details { get; set; } = default!;
    public string CardCvv { get; set; } = "N/A";
    public string CardExpirationDate { get; set; } = "N/A";
    public string CardNumber { get; set; } = "N/A";
    public string OwnerFullName { get; set; } = "N/A";
    public ECountry OwnerCountry { get; set; }
}

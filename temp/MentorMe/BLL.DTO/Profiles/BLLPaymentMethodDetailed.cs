using Base.DAL.Contracts;
using Domain.Enums;

namespace BLL.DTO.Profiles;

public class BLLPaymentMethodDetailed : IDomainEntityId
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
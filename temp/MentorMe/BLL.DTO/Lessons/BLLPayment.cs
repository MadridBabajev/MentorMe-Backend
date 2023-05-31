using Base.DAL.Contracts;
using Domain.Enums;

namespace BLL.DTO.Lessons;

public class BLLPayment: IDomainEntityId
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
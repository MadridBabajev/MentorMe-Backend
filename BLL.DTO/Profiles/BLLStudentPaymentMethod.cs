using Base.DAL.Contracts;
using Domain.Enums;

namespace BLL.DTO.Profiles;

public class BLLStudentPaymentMethod: IDomainEntityId
{
    public Guid Id { get; set; }
    public EPaymentMethod PaymentMethodType { get; set; }
    public string? Details { get; set; }
}
using System.ComponentModel.DataAnnotations;
using Base.DomainEntity;
using Domain.Enums;

namespace Domain;

public class StudentPaymentMethod: BaseDomainEntity
{
    public EPaymentMethod PaymentMethodType { get; set; }
    [MinLength(2)]
    [MaxLength(128)]
    public string? Details { get; set; }
    public bool IsBlocked { get; set; }
    
    // Nav
    public Guid StudentId { get; set; }
    public Student Student { get; set; } = default!;
}

using System.ComponentModel.DataAnnotations;
using Base.DAL.Contracts;
using Base.DomainEntity;
using Domain.Enums;

namespace Domain.Entities;

public class StudentPaymentMethod: BaseDomainEntity, IDomainEntityId
{
    public EPaymentMethod PaymentMethodType { get; set; }
    [MinLength(2)]
    [MaxLength(128)]
    public string? Details { get; set; }
    public bool IsBlocked { get; set; }
    [MinLength(1)]
    [MaxLength(32)]
    public string? CardCvv { get; set; }
    [MinLength(1)]
    [MaxLength(32)]
    public string? CardExpirationDate { get; set; }
    [MinLength(1)]
    [MaxLength(32)]
    public string? CardNumber { get; set; }

    // Nav
    public Guid StudentId { get; set; }
    public Student? Student { get; set; }
}

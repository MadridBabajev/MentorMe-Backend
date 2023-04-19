using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Base.DomainEntity;
using Domain.Enums;

namespace Domain.Entities;

public class Payment: BaseDomainEntity
{
    public DateTime Date { get; set; } = DateTime.Now;
    [Column(TypeName = "decimal(7, 2)")] 
    public double Amount { get; set; }
    [Column(TypeName = "decimal(5, 2)")] 
    public double? AdditionalFees { get; set; }
    [Column(TypeName = "decimal(5, 2)")] 
    public double? Taxes { get; set; }
    [MinLength(2)]
    [MaxLength(128)]
    public string Description { get; set; } = default!;
    public EPaymentMethod PaymentMethod { get; set; }
    public EPaymentStatus PaymentStatus { get; set; }
    
    // Nav
    public ICollection<LessonPayment>? LessonPayments { get; set; }
}

using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace Domain.Entities;

public class TutorBankingDetails
{
    public Guid Id { get; set; }
    [MinLength(8)]
    [MaxLength(16)]
    public string BankAccountNumber { get; set; } = default!;
    [MaxLength(32)]
    public string BankAccountName { get; set; } = default!;
    public bool? IsValidated { get; set; }
    public EBankAccountType BankAccountType { get; set; }
    
    // Nav
    public Guid TutorId { get; set; }
    public Tutor? Tutor { get; set; }
}

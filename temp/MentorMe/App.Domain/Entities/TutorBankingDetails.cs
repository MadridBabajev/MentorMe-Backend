using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace Domain.Entities;

public class TutorBankingDetails
{
    public Guid Id { get; set; }
    [MinLength(32)]
    [MaxLength(36)]
    public string BankAccountNumber { get; set; } = default!;
    [MinLength(2)]
    [MaxLength(32)]
    public string BankAccountName { get; set; } = default!;
    public bool IsValidated { get; set; }
    public EAccountType AccountType { get; set; }
    
    // Nav
    public Guid TutorId { get; set; }
    public Tutor? Tutor { get; set; }
}

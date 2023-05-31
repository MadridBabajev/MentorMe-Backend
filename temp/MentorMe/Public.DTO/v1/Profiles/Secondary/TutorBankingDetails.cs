using Domain.Enums;

namespace Public.DTO.v1.Profiles.Secondary;

public class TutorBankingDetails
{
    public string BankAccountNumber { get; set; } = default!;
    public string BankAccountName { get; set; } = default!;
    public EBankAccountType BankAccountType { get; set; }
}